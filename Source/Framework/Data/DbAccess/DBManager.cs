using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Config;
using LinqToDB.Data;
using LinqToDB.DataProvider;

namespace Framework.Data.DbAccess
{
    public class DBManager : IDisposable, IDBManager
    {
        private LogSettings logConfig;
        private IBaseConfiguration config;
        private ILog log;
        private IDBInfo dbInfo;
        private IDataProvider dbProvider;
        private DataConnection connection = null;
        private DataConnectionTransaction commonTransaction = null;

        public DBManager(IBaseConfiguration config, ILog log , IDBInfo dbInfo, IDataProvider dbProvider)
        {
            this.config = config;
            this.logConfig = config.LogSettings;
            this.log = log;
            this.dbInfo = dbInfo;
            ConnectionString = dbInfo.GetConnectionString();
            this.dbProvider = dbInfo.GetDBProvider();
            EnsureOpenConnection();
        }

        public string ConnectionString { get; set; }

        public LinqToDB.Data.DataConnection Connection
        {
            get
            {
                EnsureOpenConnection();
                return connection;
            }
        }

        public void Dispose()
        {
            if ((connection != null) && (connection.Connection != null) && (connection.Connection.State == ConnectionState.Open))
                connection.Close();
            connection.Dispose();
        }

        public void BeginTransaction()
        {
            log.SqlBeginTransaction(0, true);
            commonTransaction = connection.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            log.SqlRollbackTransaction(0, true);
            commonTransaction.Rollback();
        }

        public void CommitTransaction()
        {
            log.SqlCommitTransaction(0, true);
            commonTransaction.Commit();
            commonTransaction.Dispose();
        }

        private void ToggleLogging()
        {
            LinqToDB.Common.Configuration.AvoidSpecificDataProviderAPI = true;

            if (logConfig.LogSql)
            {
                DataConnection.TurnTraceSwitchOn(System.Diagnostics.TraceLevel.Verbose);
                DataConnection.OnTrace = info =>
                {
                    if (info.TraceInfoStep == TraceInfoStep.BeforeExecute)
                        return;

                    var profiledDbCommand = info.Command;

                    var result = string.Empty;
                    result = info.RecordsAffected.ToString();

                    var ptxt = new StringBuilder();
                    foreach (DbParameter param in profiledDbCommand.Parameters)
                    {
                        ptxt.Append(string.Format("{2} {0} = {1} ", param.ParameterName, param.Value, ptxt.Length > 0 ? "," : string.Empty));
                    }

                    var parameterString = ptxt.ToString();

                    if (info.Exception == null)
                        log.Sql((profiledDbCommand.CommandType == CommandType.StoredProcedure ? "SP - " : string.Empty) + profiledDbCommand.CommandText + (parameterString.Length > 0 ? ("//" + parameterString) : string.Empty), result, info.ExecutionTime ?? new TimeSpan(0));
                    else
                        log.SqlError(info.Exception, (profiledDbCommand.CommandType == CommandType.StoredProcedure ? "SP - " : string.Empty) + profiledDbCommand.CommandText + (parameterString.Length > 0 ? ("//" + parameterString) : string.Empty));
                };
            }
            else
            {
                DataConnection.TurnTraceSwitchOn(System.Diagnostics.TraceLevel.Off);
            }
        }

        private void EnsureOpenConnection()
        {
            if ((connection == null) || ((connection.Connection == null) || (connection.Connection.State == ConnectionState.Closed || connection.Connection.State == ConnectionState.Broken)))
            {
                if (connection != null)
                    connection.Dispose();
                connection = new DataConnection(dbInfo.GetDBProvider(), ConnectionString);
                connection.CommandTimeout = config.DatabaseCommandTimeout;
            }
        }
    }
}
