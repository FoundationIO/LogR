using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Config;
using LinqToDB;
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
        private int currentTransactionCount = 0;

        public DBManager(IBaseConfiguration config, ILog log, IDBInfo dbInfo)
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

        public int BeginTransaction()
        {
            if (currentTransactionCount == 0)
                commonTransaction = Connection.BeginTransaction();

            currentTransactionCount++;
            log.SqlBeginTransaction(currentTransactionCount, currentTransactionCount == 0);
            return currentTransactionCount;
        }

        public int RollbackTransaction()
        {
            if (currentTransactionCount == 0)
                throw new Exception("Begin Transaction should be called before Rollback Transaction.");

            currentTransactionCount--;
            log.SqlRollbackTransaction(currentTransactionCount, currentTransactionCount == 0);
            if (currentTransactionCount == 0)
            {
                commonTransaction.Rollback();
            }

            return currentTransactionCount;
        }

        public int CommitTransaction()
        {
            if (currentTransactionCount == 0)
                throw new Exception("Begin Transaction should be called before Commit Transaction.");

            currentTransactionCount--;
            log.SqlCommitTransaction(currentTransactionCount, currentTransactionCount == 0);
            if (currentTransactionCount == 0)
            {
                commonTransaction.Commit();
                commonTransaction.Dispose();
                commonTransaction = null;
            }

            return currentTransactionCount;
        }

        public int Delete<T>(Expression<Func<T, bool>> where)
            where T : class
        {
            return GetTable<T>().Delete(where);
        }

        public void Insert<T>(T obj)
            where T : class
        {
            var identityValue = this.Connection.InsertWithIdentity<T>(obj);
        }

        public void Insert<T>(IEnumerable<T> objs)
            where T : class
        {
            foreach (var obj in objs)
            {
                Insert(obj);
            }
        }

        public void Update<T>(T obj)
            where T : class
        {
            Connection.Update<T>(obj);
        }

        public void Update<T>(IEnumerable<T> objs)
            where T : class
        {
            foreach (var obj in objs)
            {
                Update(obj);
            }
        }

        public T First<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            return GetTable<T>().First(predicate);
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            return GetTable<T>().FirstOrDefault(predicate);
        }

        public BulkCopyRowsCopied BulkCopy<T>(IEnumerable<T> lst)
            where T : class
        {
            return this.Connection.BulkCopy<T>(lst);
        }

        public List<T> Select<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            return GetTable<T>().Where(predicate).ToList();
        }

        public List<T> SelectAll<T>()
            where T : class
        {
            return GetTable<T>().ToList<T>();
        }

        public long Count<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            return GetTable<T>().Count(predicate);
        }

        public long Count<T>()
            where T : class
        {
            return GetTable<T>().Count<T>();
        }

        public List<T> SelectByPage<T>(int pageNumber, int pageSize, out long totalItems, Expression<Func<T, bool>> predicate = null)
            where T : class
        {
            if (pageSize == 0)
                pageSize = 100;
            if (pageNumber <= 0)
                pageNumber = 1;

            var querable = GetTable<T>().Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            if (predicate != null)
            {
                querable = querable.Where(predicate);
                totalItems = GetTable<T>().Count(predicate);
            }
            else
            {
                totalItems = GetTable<T>().Count();
            }

            return querable.ToList();
        }

        public List<T> SelectByPage<T>(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate = null)
            where T : class
        {
            if (pageSize == 0)
                pageSize = 100;

            if (pageNumber <= 0)
                pageNumber = 1;

            var querable = GetTable<T>().Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            if (predicate != null)
            {
                querable = querable.Where(predicate);
            }

            return querable.ToList();
        }

        public void InsertAll<T>(List<T> list)
            where T : class
        {
            foreach (var item in list)
            {
                Insert(item);
            }
        }

        private ITable<T> GetTable<T>()
            where T : class
        {
            return Connection.GetTable<T>();
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
                connection = new DataConnection(dbInfo.GetDBProvider(), ConnectionString)
                {
                    CommandTimeout = config.DbSettings.DatabaseCommandTimeout
                };
            }
        }
    }
}
