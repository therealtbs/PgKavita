# PgKavita

An unofficial distribution of [Kavita](https://kavitareader.org) which uses [PostgreSQL](https://www.postgresql.org/) for it's database instead of SQLite.

## Caveat

Kavita makes automatic backups of your database both in regular intervals and before schema migrations. **PgKavita does not do this**. You are expected to take care of backups yourself and no one but yourself will be responsible in case of data loss.

## Why do I need this?

In general you don't. Kavita is really great out of the box and you most likely will be just fine with SQLite as the database engine. Even putting the database on an NFS share worked quite well for me, even if it's discouraged by SQLite. I wanted to consolidate where my applications' data was stored and it was not too complicated to change Kavita to suit my needs.

## What if there's an issue?

If you find an issue, please try to replicate the issue on a vanilla Kavita instance before reporting the issue to upstream. If you cannot reproduce the issue on vanilla Kavita, feel free to open an issue in this repo.

## Enough talk, how do I get this?

### Docker

Follow [Kavita's installation instructions](https://wiki.kavitareader.com/en/install/docker-install) but do the following:

1. Change the image from `kizaing/kavita` to `ghcr.io/therealtbs/pgkavita`
2. Add the following environment variable: `ConnectionStrings__DefaultConnection="Host={ENTER YOUR POSTGRES IP/HOSTNAME HERE};Port={ENTER YOUR POSTGRES PORT HERE, IT'S PROBABLY 5432};Username={USERNAME};Password={PASSWORD}"` (For a full reference on connection string parameters, check out [these docs](https://www.npgsql.org/doc/connection-string-parameters.html))

If you were already using Kavita, the Docker image will automatically migrate your data to PostgreSQL on startup. The SQLite database will be renamed to `/kavita/config/kavita.db.bak` afterwards.

### Non-Docker

While I personally use Docker, binary releases are available on the releases tab here on GitHub.

Follow [Kavita's installation instructions](https://wiki.kavitareader.com/en/install) but before running Kavita, change the connection string in `config/appsettings.json` from `Data source=config//kavita.db` to a valid connection string specifying at least a Host and Port [See the Npgsql docs for a full reference](https://www.npgsql.org/doc/connection-string-parameters.html)


## Environment Variables

Environment Variable                   | Description
---------------------------------------|------------
`ConnectionStrings__DefaultConnection` | A .NET ADO connection string specifying at least a Host and Port [See the Npgsql docs for a full reference](https://www.npgsql.org/doc/connection-string-parameters.html)
`PGUSER`                               | (Optional) The username to use for the database connection. If this is specified in the connection string, this variable is ignored.
`PGPASSWORD`                           | (Optional) The password to use for the database connection. If this is specified in the connection string, this variable is ignored.
`PGPASSFILE`                           | (Optional) The path to a [PostgreSQL password file](https://www.postgresql.org/docs/current/libpq-pgpass.html) to be used for this connection instead of `PGPASSWORD`.

For more variables, check out the [Npgsql docs](https://www.npgsql.org/doc/connection-string-parameters.html#environment-variables)