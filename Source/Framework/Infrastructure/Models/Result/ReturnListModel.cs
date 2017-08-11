using Framework.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Infrastructure.Models.Result
{
    public class ReturnListModel<T,S> where S : class
    {
        public List<T> Result { get; private set; }

        public S Search { get; private set; }

        public int ActiveTab { get; set; }

        public long TotalRecords { get; private set; }

        public bool IsSuccess { get; set; }
        public ErrorHolder ErrorHolder { get; set; }

        public ReturnListModel(S search, List<T> items, long totalItems)
        {
            Result = items;
            TotalRecords = totalItems;
            IsSuccess = true;
            Search = search;
        }

        public ReturnListModel(S search, List<T> items)
        {
            Result = items;
            TotalRecords = items.Count;
            IsSuccess = true;
            Search = search; 
        }

        public ReturnListModel(List<T> items, long totalItems):this((S)null,items, totalItems)
        {
        }

        public ReturnListModel(List<T> items) : this((S)null, items)
        {
        }

        public ReturnListModel(S search, Exception ex)
        {
            IsSuccess = false;
            ErrorHolder = ErrorHolder.Create(ex);
        }

        public ReturnListModel(S search, string errorMsg, Exception ex = null)
        {
            IsSuccess = false;
            ErrorHolder = ErrorHolder.Create(errorMsg, ex);
        }

        public ReturnListModel(S search, string errorMsg, List<ErrorItem> errorList)
        {
            IsSuccess = false;
            ErrorHolder = ErrorHolder.Create(errorMsg, errorList);
        }
        public ReturnListModel(Exception ex): this((S)null, ex)
        {
        }

        public ReturnListModel(string errorMsg, Exception ex = null): this((S)null, errorMsg, ex)
        {
        }

        public ReturnListModel(string errorMsg, List<ErrorItem> errorList) : this((S)null, errorMsg, errorList)
        {
        }

    }
}
