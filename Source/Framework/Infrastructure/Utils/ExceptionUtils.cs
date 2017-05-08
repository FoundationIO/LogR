using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Utils
{
    public static class ExceptionUtils
    {
        private static void GetInnerExceptionMsg(this Exception ex, ref List<string> errorList, bool withStackTrace)
        {
            errorList.Add(ex.Message + (withStackTrace ? (String.Format(" ST - {0}", ex.StackTrace)) : ""));
            if (ex.InnerException != null)
                GetInnerExceptionMsg(ex.InnerException, ref errorList, withStackTrace);
        }

        public static string RecursivelyGetExceptionMessage(this Exception ex, bool withStackTrace = true)
        {
            if (ex == null)
                return "";
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

    }
}
