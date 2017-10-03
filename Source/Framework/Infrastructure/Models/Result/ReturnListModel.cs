using System;
using System.Collections.Generic;
using Framework.Infrastructure.Exceptions;

namespace Framework.Infrastructure.Models.Result
{
    public class ReturnListModel<TModel,TSearch> : IReturnModel
        where TSearch : class
    {
        public ReturnListModel(TSearch search, List<TModel> items, long totalItems)
        {
            Model = items;
            TotalRecords = totalItems;
            IsSuccess = true;
            Search = search;
        }

        public ReturnListModel(TSearch search, List<TModel> items)
        {
            Model = items;
            TotalRecords = items.Count;
            IsSuccess = true;
            Search = search;
        }

        public ReturnListModel(List<TModel> items, long totalItems)
            : this((TSearch)null, items, totalItems)
        {
        }

        public ReturnListModel(List<TModel> items)
            : this((TSearch)null, items)
        {
        }

        public ReturnListModel(TSearch search, Exception ex)
        {
            IsSuccess = false;
            ErrorHolder = new Error(ex);
        }

        public ReturnListModel(TSearch search, string errorMsg, Exception ex = null)
        {
            IsSuccess = false;
            ErrorHolder = new Error(errorMsg, ex);
        }

        public ReturnListModel(TSearch search, string errorMsg, List<ErrorItem> errorList)
        {
            IsSuccess = false;
            ErrorHolder = new Error(errorMsg, errorList);
        }

        public ReturnListModel(Exception ex)
            : this((TSearch)null, ex)
        {
        }

        public ReturnListModel(string errorMsg, Exception ex = null)
            : this((TSearch)null, errorMsg, ex)
        {
        }

        public ReturnListModel(string errorMsg, List<ErrorItem> errorList)
            : this((TSearch)null, errorMsg, errorList)
        {
        }

        public List<TModel> Model { get; private set; }

        public TSearch Search { get; private set; }

        public int ActiveTab { get; set; }

        public long TotalRecords { get; private set; }

        public bool IsSuccess { get; set; }

        public Error ErrorHolder { get; set; }

        public int HttpCode { get; set; }
    }
}
