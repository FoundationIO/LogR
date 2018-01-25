using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
using LogR.Common.Constants;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Repository.Log
{
    public abstract class BaseLogRepository
    {
        protected ILog log;

        protected IAppConfiguration config;

        public BaseLogRepository(ILog log, IAppConfiguration config)
        {
            this.log = log;
            this.config = config;
        }

        public void SaveLog(RawLogData data)
        {
            if (data == null)
            {
                log.Error("Error ");
                return;
            }

            SaveLog(new List<RawLogData> { data });
        }

        public abstract void SaveLog(List<RawLogData> data);

        // Parameters
        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetAppNames(StoredLogType logType, BaseSearchCriteria search)
        {
            return GetDistinctColumns(logType, search, x => x.App, "Application");
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetMachineNames(StoredLogType logType, BaseSearchCriteria search)
        {
            return GetDistinctColumns(logType, search, x => x.MachineName, "Machine");
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetUserNames(StoredLogType logType, BaseSearchCriteria search)
        {
            return GetDistinctColumns(logType, search, x => x.UserIdentity, "User");
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetSeverityNames(StoredLogType logType, BaseSearchCriteria search)
        {
            return GetDistinctColumns(logType, search, x => x.Severity, "Severity");
        }

        protected abstract ReturnListWithSearchModel<string, BaseSearchCriteria> GetDistinctColumns(StoredLogType logType, BaseSearchCriteria search, Expression<Func<AppLog, string>> selector, string columnType);

        protected IQueryable<T> WhereEquals<T>(IQueryable<T> source, string member, object value)
        {
            var item = Expression.Parameter(typeof(T), "item");
            var memberValue = member.Split('.').Aggregate((Expression)item, Expression.PropertyOrField);
            var memberType = memberValue.Type;
            if (value != null && value.GetType() != memberType)
                value = Convert.ChangeType(value, memberType);
            var condition = Expression.Equal(memberValue, Expression.Constant(value, memberType));
            var predicate = Expression.Lambda<Func<T, bool>>(condition, item);
            return source.Where(predicate);
        }

        protected IQueryable<TSource> ApplySort<TSource>(IQueryable<TSource> query, string propertyName, bool isAsc, string defaultSortByField)
        {
            if (propertyName == null)
                return query;

            var entityType = typeof(TSource);

            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var orderByFunction = "OrderBy";
            if (isAsc)
                orderByFunction = "OrderByDescending";

            var enumarableType = typeof(System.Linq.Queryable);
            MethodInfo method = enumarableType.GetMethods()
                 .Where(m => m.Name == orderByFunction && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable<TSource>)genericMethod
                    .Invoke(genericMethod, new object[] { query, selector });

            return newQuery;
        }

        protected IQueryable<T> AddFilters<T>(IQueryable<T> source, List<SearchTerm> searchTerms)
        {
            if (searchTerms == null || searchTerms.Count == 0)
                return source;

            foreach (var term in searchTerms)
            {
                string member = term.Key;
                object value = term.Value;

                var item = Expression.Parameter(typeof(T), "item");
                var memberValue = member.Split('.').Aggregate((Expression)item, Expression.PropertyOrField);
                var memberType = memberValue.Type;
                try
                {
                    if (value != null && value.GetType() != memberType)
                        value = Convert.ChangeType((object)value, memberType);
                }
                catch
                {
                    log.Warn($"Key : {term.Key}, Value : {term.Value}, Operator : {term.Operator} conversion error - skipping item");
                }

                Expression condition;
                switch (term.Operator)
                {
                    case SearchFieldContants.Operators.Is:
                    case SearchFieldContants.Operators.EqualTo:
                    case SearchFieldContants.Operators.NotEqualTo:
                        {
                            condition = Expression.Equal(memberValue, Expression.Constant(value, memberType));

                            if (term.Operator == SearchFieldContants.Operators.NotEqualTo)
                                condition = Expression.Not(condition);
                            break;
                        }

                    case SearchFieldContants.Operators.GreaterThan:
                        {
                            if (memberType == typeof(DateTime))
                            {
                                var dt = Convert.ToDateTime(value);
                                value = dt.Date.StartOfDay();
                            }

                            condition = Expression.GreaterThan(memberValue, Expression.Constant(value, memberType));
                            break;
                        }

                    case SearchFieldContants.Operators.GreaterThanOrEqualTo:
                        {
                            if (memberType == typeof(DateTime))
                            {
                                var dt = Convert.ToDateTime(value);
                                value = dt.Date.StartOfDay();
                            }

                            condition = Expression.GreaterThanOrEqual(memberValue, Expression.Constant(value, memberType));
                            break;
                        }

                    case SearchFieldContants.Operators.LessThan:
                        {
                            if (memberType == typeof(DateTime))
                            {
                                var dt = Convert.ToDateTime(value);
                                value = dt.Date.EndOfDay();
                            }

                            condition = Expression.LessThan(memberValue, Expression.Constant(value, memberType));
                            break;
                        }

                    case SearchFieldContants.Operators.LessThanOrEqualTo:
                        {
                            if (memberType == typeof(DateTime))
                            {
                                var dt = Convert.ToDateTime(value);
                                value = dt.Date.EndOfDay();
                            }

                            condition = Expression.LessThanOrEqual(memberValue, Expression.Constant(value, memberType));
                            break;
                        }

                    case SearchFieldContants.Operators.Contains:
                    case SearchFieldContants.Operators.NotContains:
                        {
                            var propertyExp = Expression.Property(item, term.Key);
                            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            var someValue = Expression.Constant(term.Value, typeof(string));
                            condition = Expression.Call(propertyExp, method, someValue);

                            if (term.Operator == SearchFieldContants.Operators.NotContains)
                                condition = Expression.Not(condition);
                            break;
                        }

                    case SearchFieldContants.Operators.StartsWith:
                        {
                            condition = Expression.LessThanOrEqual(memberValue, Expression.Constant(value, memberType));
                            break;
                        }

                    case SearchFieldContants.Operators.EndsWith:
                        {
                            condition = Expression.LessThanOrEqual(memberValue, Expression.Constant(value, memberType));
                            break;
                        }

                    default:
                        continue;
                }

                /*case SearchFieldContants.Operators.
                        public const string IsNot = "is not";
                        public const string Contains = "contains";
                        public const string NotContains = "not contains";
                        public const string StartsWith = "starts with";
                        public const string EndsWith = "ends with";

                        public const string GreaterThan = ">";
                        public const string LessThan = "<";
                        public const string GreaterThanOrEqualTo = ">=";
                        public const string LessThanOrEqualTo = "<=";
                        public const string EqualTo = "=";
                        public const string NotEqualTo = "!=";
                        */
                var predicate = Expression.Lambda<Func<T, bool>>(condition, item);
                source = source.Where(predicate);
            }

            return source;
        }

        protected T GetLogFromRawLog<T>(StoredLogType logType, string applicationId, string message)
            where T : AppLog
        {
            var outerData = JsonUtils.Deserialize<RawLogData>(message);
            if (outerData == null)
            {
                throw new Exception("Unable to deserialize the log message -  " + message);
            }

            var item = JsonUtils.Deserialize<T>(outerData.Data);

            if (item == null)
            {
                throw new Exception("Unable to deserialize the log internal message -  " + outerData.Data);
            }

            item.LogId = Guid.NewGuid();
            item.LogType = (int)logType;
            item.ApplicationId = applicationId;

            if (item.Longdate.IsInvalidDate())
                item.Longdate = DateTime.UtcNow;
            item.ReceivedDate = outerData.ReceiveDate;

            if (item.ReceivedDate.IsInvalidDate())
                item.ReceivedDate = DateTime.UtcNow;

            item.LongdateAsTicks = item.Longdate.Ticks;
            item.ReceivedDateAsTicks = item.ReceivedDate.Ticks;
            return item;
        }

        protected List<AppLog> GetAppLogsFromRawData(List<RawLogData> data)
        {
            var lst = new List<AppLog>();
            if (data != null)
            {
                foreach (var message in data)
                {
                    var item = this.GetLogFromRawLog<AppLog>(message.Type, message.ApplicationId, message.Data);
                    lst.Add(item);
                }
            }

            return lst;
        }
    }
}