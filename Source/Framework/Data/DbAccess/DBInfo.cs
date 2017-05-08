using FluentMigrator.Runner.Processors;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.MySql;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.SqlServer;
using Framework.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;

namespace Framework.Data.DbAccess
{
    public class DBInfo : IDBInfo
    {
        private static SqlServerDataProvider sqlServerProvider = new SqlServerDataProvider("default", SqlServerVersion.v2008);
        private static SQLiteDataProvider sqlite3Provider = new SQLiteDataProvider();
        private static MySqlDataProvider mySqlProvider = new MySqlDataProvider();
        public IBaseConfiguration config;

        public DBInfo(IBaseConfiguration config)
        {
            this.config = config;
        }

        public string GetConnectionString()
        {
            int workerThreads, completionPortThreads;
            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);

            var connectionStr = "";
            switch (config.DatabaseType)
            {
                case DBType.MYSQL:
                    {
                        connectionStr = $"Server={config.DatabaseServer};Database={config.DatabaseName};Uid={config.DatabaseUserName};Pwd={config.DatabasePassword};";
                        break;
                    }
                case DBType.SQLSERVER:
                    {
                        connectionStr = $"Server={config.DatabaseServer};Initial Catalog={config.DatabaseName};Persist Security Info=True;User ID={config.DatabaseUserName};Password={config.DatabasePassword};MultipleActiveResultSets=False;Application Name={config.AppName};Max Pool Size={workerThreads};";
                        break;
                    }
                case DBType.SQLITE3:
                    {
                        connectionStr = $"Data Source={config.DatabaseName}; Version=3;PRAGMA journal_mode=WAL;";
                        break;
                    }
                default:
                    {
                        throw new Exception($"Unable to get Configuration string, Unknown Database type specified in the configuration {config.DatabaseType}");
                    }
            }
            return connectionStr;
        }

        public MigrationProcessorFactory GetMigrationProcessorFactory()
        {
            switch (config.DatabaseType)
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
                        throw new Exception($"Unable to get Migration Process Factory, Unknown Database type specified in the configuration {config.DatabaseType}");
                    }
            }
        }

        public IDataProvider GetDBProvider()
        {
            switch (config.DatabaseType)
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
                        throw new Exception($"Unable to get DB Provider, Unknown Database type specified in the configuration {config.DatabaseType}");
                    }
            }
            throw new Exception(String.Format("DB Type {0} is not supported yet", config.DatabaseType));
        }

    }
}
