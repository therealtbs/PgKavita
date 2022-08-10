using API.Data;
using Microsoft.EntityFrameworkCore;
using Kavita.Common;
using Microsoft.Data.Sqlite;
using PgKavita.DataMigrationTool;

Console.WriteLine("PgKavita Data Migration Tool");
Console.WriteLine();

var sqliteFilePath = (args.Length > 0 ? args[0] : "config/kavita.db");

using var sqliteConn = new SqliteConnection($"Data Source={sqliteFilePath};Mode=ReadOnly");

await sqliteConn.OpenAsync();

using (var listMigrationsCommand = sqliteConn.CreateCommand()) {
    listMigrationsCommand.CommandText = "SELECT MigrationId FROM __EFMigrationsHistory;";
    var executedMigrations = await listMigrationsCommand.ToStringList();
    if (!(executedMigrations).SequenceEqual(Migrations.ExpectedMigrations)) {
        Console.WriteLine("Cannot proceed with migration: SQLite database does not have all expected migrations. Please let vanilla Kavita upgrade the database fully before switching to PgKavita.");
        return 1;
    }
}

var pgConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? (args.Length > 1 ? args[1] :  Configuration.DatabasePath);

var pgOptions = new DbContextOptionsBuilder();
System.AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
pgOptions.UseNpgsql(pgConnectionString);
pgOptions.EnableDetailedErrors();
pgOptions.EnableSensitiveDataLogging();

using var pgContext = new DataContext(pgOptions.Options);

Console.WriteLine("Ensuring DB structure is valid in target DB");
await pgContext.Database.MigrateAsync(); // Create DB structure in Postgres DB
Console.WriteLine("Done with schema migrations");

var pgConn = pgContext.Database.GetDbConnection() as Npgsql.NpgsqlConnection;
if (pgConn == null) {
    Console.WriteLine("Error: pgContext is not using Npgsql");
    return 1;
}
await pgConn.OpenAsync();

var settings = await pgConn.GetAllData("ServerSetting");
if (settings.Rows.Count > 0) {
    Console.WriteLine("Cannot proceed with migration: PostgreSQL database has already been seeded by PgKavita. Either remove/rename the SQLite DB if the migration was already successful or truncate the PostgreSQL database.");
    return 1;
}

List<string> pgTables;
using (var listPgTablesCommand = pgConn.CreateCommand()) {
    listPgTablesCommand.CommandText = "SELECT tablename FROM pg_tables WHERE schemaname = 'public' and tablename <> '__EFMigrationsHistory';";
    pgTables = await listPgTablesCommand.ToStringList();
}

List<string> sqliteTables;
using (var listTablesCommand = sqliteConn.CreateCommand()){ 
    listTablesCommand.CommandText = @"
        SELECT name FROM sqlite_schema
        WHERE type='table'
        ORDER BY name;
    ";
    sqliteTables = await listTablesCommand.ToStringList();
}
var tablesToMigrate = sqliteTables.Intersect(pgTables);

var transaction = await pgConn.BeginTransactionAsync();
try {
    foreach (var table in tablesToMigrate) {
        Console.WriteLine($"Migrating data for table {table}");
        var data = await sqliteConn.GetAllData(table);
        await pgConn.SetTriggers(table, false);
        await pgConn.WriteAllData(table, data);
        await pgConn.SetTriggers(table, true);
    }
    await transaction.CommitAsync();
} catch (Exception ex) {
    await transaction.RollbackAsync();
    throw ex;
} finally {
    await transaction.DisposeAsync();
}

var sqliteSequences = await sqliteConn.GetAllData("sqlite_sequence");

for (var i = 0; i < sqliteSequences.Rows.Count; i++) {
    var row = sqliteSequences.Rows[i];
    if ((long)row["seq"] == 0) {
        continue;
    }
    Console.WriteLine($"Setting seq pointer for table {row["name"]}");
    using (var setSeqCommand = pgConn.CreateCommand()) {
        setSeqCommand.CommandText = "SELECT setval(pg_get_serial_sequence($1, 'Id'), $2)";
        setSeqCommand.Parameters.AddWithValue($"\"{row["name"]}\"");
        setSeqCommand.Parameters.AddWithValue(row["seq"]);
        await setSeqCommand.ExecuteScalarAsync();
    }
}
Console.WriteLine($"Renaming {sqliteFilePath} to {(sqliteFilePath + ".bak")}");
File.Move(sqliteFilePath, sqliteFilePath + ".bak", true);


Console.WriteLine();
Console.WriteLine("Migration Done, enjoy!");
return 0;