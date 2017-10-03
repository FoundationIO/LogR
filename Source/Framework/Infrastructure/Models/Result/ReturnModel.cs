using System;
using System.Collections.Generic;
using Framework.Infrastructure.Exceptions;

namespace Framework.Infrastructure.Models.Result
{
    public class ReturnModel<T> : IReturnModel
    {
        public ReturnModel(T result)
        {
            Model = result;
            IsSuccess = true;
        }

        public ReturnModel(Exception ex)
        {
            IsSuccess = false;
            ErrorHolder = new Error(ex);
        }

        public ReturnModel(string errorMsg, Exception ex = null)
        {
            IsSuccess = false;
            ErrorHolder = new Error(errorMsg, ex);
        }

        public ReturnModel(string errorMsg, List<ErrorItem> errorList)
        {
            IsSuccess = false;
            ErrorHolder = new Error(errorMsg, errorList);
        }

        public T Model { get; set; }

        public bool IsSuccess { get; set; }

        public Error ErrorHolder { get; set; }

        public int ActiveTab { get; set; }

        public int HttpCode { get; set; }
    }
}
