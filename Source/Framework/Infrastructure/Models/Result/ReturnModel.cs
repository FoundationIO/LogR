using Framework.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Infrastructure.Models.Result
{
    public class ReturnModel<T>
    {
        public T Result { get; set; }
        public bool IsSuccess { get; set; }
        public ErrorHolder ErrorHolder { get; set; }
        public int ActiveTab { get; set; }

        public ReturnModel(T result)
        {
            Result = result;
            IsSuccess = true;
        }

        public ReturnModel(Exception ex)
        {
            IsSuccess = false;
            ErrorHolder = ErrorHolder.Create(ex);
        }

        public ReturnModel(string errorMsg, Exception ex = null)
        {
            IsSuccess = false;
            ErrorHolder = ErrorHolder.Create(errorMsg, ex);
        }

        public ReturnModel(string errorMsg, List<ErrorItem> errorList)
        {
            IsSuccess = false;
            ErrorHolder = ErrorHolder.Create(errorMsg, errorList);
        }
    }
}
