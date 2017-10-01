using System;
using System.Threading;
using FluentMigrator.Runner.Processors;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Constants;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.MySql;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.SqlServer;

namespace Framework.Data.DbAccess
{
    public class DBInfo : IDBInfo
    {
        private static SqlServerDataProvider sqlServerProvider = new SqlServerDataProvider("default", SqlServerVersion.v2008);
        private static SQLiteDataProvider sqlite3Provider = new SQLiteDataProvider();
        private static MySqlDataProvider mySqlProvider = new MySqlDataProvider();
        private IBaseConfiguration config;

        public DBInfo(IBaseConfiguration config)
        {
            this.config = config;
        }

        public virtual string GetConnectionString()
        {
            ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);

            var connectionStr = string.Empty;

            var dbType = (config.DbSettings.DatabaseType ?? string.Empty).Trim().ToLower();
            switch (dbType)
            {
                case DBType.MYSQL:
                    {
                        if (config.DbSettings.DatabaseUseIntegratedLogin)
                            connectionStr = $"IntegratedSecurity=yes;Server={config.DbSettings.DatabaseServer};Database={config.DbSettings.DatabaseName};";
                        else
                            connectionStr = $"Server={config.DbSettings.DatabaseServer};Database={config.DbSettings.DatabaseName};Uid={config.DbSettings.DatabaseUserName};Pwd={config.DbSettings.DatabasePassword};";
                        break;
                    }

                case DBType.SQLSERVER:
                    {
                        if (config.DbSettings.DatabaseUseIntegratedLogin)
                            connectionStr = $"Integrated Security=true;Server={config.DbSettings.DatabaseServer};Initial Catalog={config.DbSettings.DatabaseName};Persist Security Info=True;MultipleActiveResultSets =False;Application Name={config.AppName};Max Pool Size={workerThreads};";
                        else
                            connectionStr = $"Server={config.DbSettings.DatabaseServer};Initial Catalog={config.DbSettings.DatabaseName};Persist Security Info=True;User ID={config.DbSettings.DatabaseUserName};Password={config.DbSettings.DatabasePassword};MultipleActiveResultSets=False;Application Name={config.AppName};Max Pool Size={workerThreads};";
                        break;
                    }

                case DBType.SQLITE3:
                    {
                        connectionStr = $"Data Source={config.DbSettings.DatabaseName}; Version=3;PRAGMA journal_mode=WAL;";
                        break;
                    }

                default:
                    {
                        throw new Exception($"Unable to get Configuration string, Unknown Database type specified in the configuration {config.DbSettings.DatabaseType}");
                    }
            }

            return connectionStr;
        }

        public virtual MigrationProcessorFactory GetMigrationProcessorFactory()
        {
            var dbType = (config.DbSettings.DatabaseType ?? string.Empty).Trim().ToLower();
            switch (dbType)
            {
                case DBType.MYSQL:
                    {
                        return new FluentMigrator.Runner.Processors.MySql.MySqlProcessorFactory();
                    }

                case DBType.SQLSERVER:
                    {
                        return new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();
                    }

                case DBType.SQLITE3:
                    {
                        return new FluentMigrator.Runner.Processors.SQLite.SQLiteProcessorFactory();
                    }

                default:
                    {
                        throw new Exception($"Unable to get Migration Process Factory, Unknown Database type specified in the configuration {config.DbSettings.DatabaseType}");
                    }
            }
        }

        public virtual IDataProvider GetDBProvider()
        {
            var dbType = (config.DbSettings.DatabaseType ?? string.Empty).Trim().ToLower();
            switch (dbType)
            {
                case DBType.MYSQL:
                    {
                        return mySqlProvider;
                    }

                case DBType.SQLSERVER:
                    {
                        return sqlServerProvider;
                    }

                case DBType.SQLITE3:
                    {
                        return sqlite3Provider;
                    }

                default:
                    {
                        throw new Exception($"Unable to get DB Provider, Unknown Database type specified in the configuration {config.DbSettings.DatabaseType}");
                    }
            }

            throw new Exception(string.Format("DB Type {0} is not supported yet", config.DbSettings.DatabaseType));
        }
    }
}
