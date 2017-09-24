using System;
using System.Collections.Generic;

namespace Framework.Infrastructure.Exceptions
{
    public class Error
    {
        public Error()
        {
        }

        public Error(string friendlyMessage)
        {
            FriendlyMessage = friendlyMessage;
            InternalErrorMessage = friendlyMessage;
            ErrorItemList = null;
            Exception = null;
        }

        public Error(string friendlyMessage, string internalErrorMessage = null, List<ErrorItem> errorItemList = null, Exception ex = null)
        {
            FriendlyMessage = friendlyMessage;
            InternalErrorMessage = internalErrorMessage ?? friendlyMessage;
            ErrorItemList = errorItemList;
            Exception = ex;
        }

        public Error(string friendlyMessage, List<ErrorItem> errorItemList = null, Exception ex = null)
        {
            FriendlyMessage = friendlyMessage;
            InternalErrorMessage = friendlyMessage;
            ErrorItemList = errorItemList;
            Exception = null;
        }

        public Error(Exception ex)
        {
            FriendlyMessage = ex.Message;
            InternalErrorMessage = ex.Message;
            ErrorItemList = null;
            Exception = ex;
        }

        public Error(string friendlyMessage, Exception ex)
        {
            FriendlyMessage = friendlyMessage;
            InternalErrorMessage = friendlyMessage;
            ErrorItemList = null;
            Exception = ex;
        }

        public string FriendlyMessage { get; set; }

        public string InternalErrorMessage { get; set; }

        public List<ErrorItem> ErrorItemList { get; set; }

        public Exception Exception { get; set; }
    }
}
