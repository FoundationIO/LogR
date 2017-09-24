using System;
using System.Collections.Generic;

namespace Framework.Infrastructure.Utils
{
    public static class ExceptionUtils
    {
        public static string RecursivelyGetExceptionMessage(this Exception ex, bool withStackTrace = true)
        {
            if (ex == null)
                return string.Empty;
            var errorList = new List<string>();
            GetInnerExceptionMsg(ex, ref errorList, withStackTrace);
            return StringUtils.ToString(errorList, "\n");
        }

        public static List<string> RecursivelyGetExceptionMessageList(this Exception ex, bool withStackTrace = true)
        {
            var errorList = new List<string>();
            if (ex != null)
            {
                GetInnerExceptionMsg(ex, ref errorList, withStackTrace);
            }

            return errorList;
        }

        private static void GetInnerExceptionMsg(this Exception ex, ref List<string> errorList, bool withStackTrace)
        {
            errorList.Add(ex.Message + (withStackTrace ? string.Format(" ST - {0}", ex.StackTrace) : string.Empty));
            if (ex.InnerException != null)
                GetInnerExceptionMsg(ex.InnerException, ref errorList, withStackTrace);
        }
    }
}
