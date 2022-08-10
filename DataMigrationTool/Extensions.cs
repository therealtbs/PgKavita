using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace PgKavita.DataMigrationTool
{

    public static class Extensions
    {

        public static async Task<List<string>> ToStringList(this DbCommand command)
        {
            var list = new List<string>();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }
            return list;
        }

        public static async Task SetConstraintsDeferred(this DbConnection conn, string table, bool deferred)
        {
            var constraintChanges = new List<string>();
            using (var constraints = await conn.GetSchemaAsync("ForeignKeys", new string[] { "", "", table })) {
                for (var i = 0; i < constraints.Rows.Count; i++) {
                     constraintChanges.Add($"ALTER CONSTRAINT \"{(constraints.Rows[i]["CONSTRAINT_NAME"])}\" {(deferred ? "DEFERRABLE INITIALLY DEFERRED" : "NOT DEFERRABLE")}");
                }
            }
            if (constraintChanges.Count == 0) {
                return;
            }
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"ALTER TABLE \"{table}\" {String.Join(", ", constraintChanges)};";
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task<DataTable> GetAllData(this DbConnection conn, string table)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM \"{table}\";";
            var dt = new DataTable();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                dt.Load(reader);
            }
            return dt;
        }

        public static async Task WriteAllData(this Npgsql.NpgsqlConnection conn, string table, DataTable data)
        {

            var GlobalTypeMapper = typeof(Npgsql.NpgsqlConnection).Assembly.GetType("Npgsql.TypeMapping.GlobalTypeMapper");
            var typeMapper = GlobalTypeMapper?.GetMethod("DataTypeNameToNpgsqlDbType", BindingFlags.Public | BindingFlags.Static)?.CreateDelegate<Func<string, NpgsqlTypes.NpgsqlDbType>>();
            if (typeMapper == null) {
                throw new Exception("Npgsql internals have changed");
            }
            var columns = await conn.GetSchemaAsync("columns", new string[] { "", "", table });
            var fields = from DataColumn column in data.Columns select $"\"{column.ColumnName}\"";
            using (var writer = await conn.BeginBinaryImportAsync($"COPY \"{table}\" ({(String.Join(", ", fields))}) FROM STDIN (FORMAT BINARY)"))
            {
                foreach (DataRow row in data.Rows)
                {
                    await writer.StartRowAsync();
                    for (var i = 0; i < fields.Count(); i++)
                    {
                        
                        var val = row[i];
                        var dbType = (from DataRow col in columns.Rows where col["column_name"] as string == data.Columns[i].ColumnName select col["data_type"] as string).First();
                        var type = typeMapper(dbType);
                        try {
                            switch (type) {
                                case NpgsqlTypes.NpgsqlDbType.Boolean:
                                    val = (long)val == 1 ? true : false;
                                    break;
                                case NpgsqlTypes.NpgsqlDbType.Timestamp:
                                    DateTime tsValue;
                                    if (DateTime.TryParse(val.ToString(), out tsValue)) {
                                        val = tsValue;
                                    }
                                    break;
                            }
                            await writer.WriteAsync(val, type);
                        } catch (Exception ex) {
                            Console.WriteLine($"Exception with Data in table {table}, {fields.ElementAt(i)} ({dbType}) = {val}");
                            throw ex;
                        }
                        
                    }
                }
                await writer.CompleteAsync();
            }
        }

    }

}