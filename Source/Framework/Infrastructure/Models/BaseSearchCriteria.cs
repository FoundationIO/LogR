using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Models
{
    public class BaseSearchCriteria
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Keyword { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
        public int CurrentRows { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public BaseSearchCriteria()
        {
            PageSize = 100;
            NumericPageCount = 10;
            Page = 1;
            TotalRowCount = -1;
            CurrentRows = -1;
        }

        public void FixDefaultValues()
        {
            if ((this.ToDate == null) && (this.FromDate == null))
            {
                this.FromDate = DateTime.Today.AddDays(-7);
                this.ToDate = DateTime.Today;
            }
            else if ((this.ToDate == null) && (this.FromDate != null))
            {
                this.ToDate = DateTime.Today;
            }
            else if ((this.ToDate != null) && (this.FromDate == null))
            {
                this.FromDate = this.ToDate.Value.AddDays(-7);
            }
        }

        private void ValidateMandatoryParams()
        {
            if (TotalRowCount == -1)
                throw new Exception("TotalRowCount should be set to the criteria object before sending to the View");

            if (CurrentRows == -1)
                throw new Exception("CurrentRows should be set to the criteria object before sending to the View");

        }

        public int CurrentPageStartRow()
        {
            ValidateMandatoryParams();
            if (TotalRowCount == 0)
                return 0;

            int result = (Page - 1) * PageSize;
            return result + 1;
        }

        public int CurrentPageEndRow()
        {
            ValidateMandatoryParams();
            if (TotalRowCount == 0)
                return 0;

            int result = ((Page - 1) * PageSize) + CurrentRows;
            return result;
        }


        public long PageCount()
        {
            ValidateMandatoryParams();
            long pageCount = TotalRowCount % this.PageSize == 0 ? TotalRowCount / this.PageSize : TotalRowCount / this.PageSize + 1;
            return Math.Max(pageCount, 1);
        }


        public long TotalRowCount { get; set; }

        public int NumericPageCount { get; set; }

    }
}
