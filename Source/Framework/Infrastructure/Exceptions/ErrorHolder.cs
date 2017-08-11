using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Infrastructure.Exceptions
{
    public class ErrorHolder
    {
        public string FriendlyMessage { get; set; }
        public string InternalErrorMessage { get; set; }
        public List<ErrorItem> ErrorItemList { get; set; }
        public Exception Exception { get; set; }

        public static ErrorHolder Create(string friendlyMessage)
        {
            return new ErrorHolder { FriendlyMessage = friendlyMessage, InternalErrorMessage = friendlyMessage, ErrorItemList = null, Exception = null };
        }

        public static ErrorHolder Create(string friendlyMessage, string internalErrorMessage = null, List<ErrorItem> errorItemList = null, Exception ex = null)
        {
            return new ErrorHolder { FriendlyMessage = friendlyMessage, InternalErrorMessage = internalErrorMessage ?? friendlyMessage, ErrorItemList = errorItemList, Exception = ex };
        }

        public static ErrorHolder Create(string friendlyMessage, List<ErrorItem> errorItemList = null, Exception ex = null)
        {
            return new ErrorHolder { FriendlyMessage = friendlyMessage, InternalErrorMessage = friendlyMessage, ErrorItemList = errorItemList, Exception = null };
        }

        public static ErrorHolder Create(Exception ex)
        {
            return new ErrorHolder { FriendlyMessage = ex.Message, InternalErrorMessage = ex.Message, ErrorItemList = null, Exception = ex };
        }

        public static ErrorHolder Create(string friendlyMessage, Exception ex)
        {
            return new ErrorHolder { FriendlyMessage = friendlyMessage, InternalErrorMessage = friendlyMessage, ErrorItemList = null, Exception = ex };
        }
    }
}
