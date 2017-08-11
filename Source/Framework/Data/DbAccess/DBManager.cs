using LinqToDB.Data;
using LinqToDB.DataProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;

namespace Framework.Data.DbAccess
{
    public class DBManager : IDisposable, IDBManager
    {
        IBaseConfiguration config;
        ILog log;
        IDBInfo dbInfo;
        IDataProvider dbProvider;
        private DataConnection connection = null;

        public string ConnectionString { get; set; }

        private DataConnectionTransaction commonTransaction = null;

        private static bool loggingToggled = false;

        public DBManager(IBaseConfiguration config, ILog log , IDBInfo dbInfo, IDataProvider dbProvider)
        {
            this.config = config;
            this.log = log;
            this.dbInfo = dbInfo;
            ConnectionString = dbInfo.GetConnectionString();
            this.dbProvider = dbInfo.GetDBProvider();
            EnsureOpenConnection();
        }

        private void ToggleLogging()
        {
            if (loggingToggled != true)
            LinqToDB.Common.Configuration.AvoidSpecificDataProviderAPI = true;

            if (config.LogSqlEnable)
            {
                DataConnection.TurnTraceSwitchOn(System.Diagnostics.TraceLevel.Verbose);
                DataConnection.OnTrace = delegate (TraceInfo info)
                {
                    if (info.BeforeExecute)
                        return;

                    var profiledDbCommand = info.Command;

                    var result = "";
                    result = info.RecordsAffected.ToString();

                    var ptxt = new StringBuilder();
                    foreach (DbParameter param in profiledDbCommand.Parameters)
                    {
                        ptxt.Append(String.Format("{2} {0} = {1} ", param.ParameterName, param.Value, ptxt.Length > 0 ? "," : ""));
                    }

                    var parameterString = ptxt.ToString();

                    if (info.Exception == null)
                        log.Sql((profiledDbCommand.CommandType == CommandType.StoredProcedure ? "SP - " : "") + profiledDbCommand.CommandText + (parameterString.Length > 0 ? ("//" + parameterString) : ""), result, info.ExecutionTime ?? new TimeSpan(0));
                    else
                        log.SqlError(info.Exception, (profiledDbCommand.CommandType == CommandType.StoredProcedure ? "SP - " : "") + profiledDbCommand.CommandText + (parameterString.Length > 0 ? ("//" + parameterString) : ""));
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

        public LinqToDB.Data.DataConnection Connection
        {
            get { EnsureOpenConnection(); return connection; }
        }
    }

}
