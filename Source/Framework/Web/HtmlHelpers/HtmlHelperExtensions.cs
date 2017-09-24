using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Framework.Web.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString PageDropDownList(this IHtmlHelper html, string id, string selectedItem, string className, string style, string callbackFunction = null, bool isPositionRelative = false)
        {
            List<TextWithValue> items = new List<TextWithValue>();
            items.Add(new TextWithValue { Text = "10", Value = "10" });
            items.Add(new TextWithValue { Text = "25", Value = "25" });
            items.Add(new TextWithValue { Text = "50", Value = "50" });
            items.Add(new TextWithValue { Text = "100", Value = "100" });
            items.Add(new TextWithValue { Text = "500", Value = "500" });
            items.Add(new TextWithValue { Text = "1000", Value = "1000" });
            return DropDownListEx(html, id, items, selectedItem, className, style, callbackFunction, isPositionRelative);
        }

        public static HtmlString DropDownListEx(this IHtmlHelper html, string id, List<TextWithValue> items, string selectedItem, string className, string style, string callbackFunction = null, bool isPositionRelative = false)
        {
            StringBuilder sb = new StringBuilder();
            var defaultText = string.Empty;
            var defaultValue = string.Empty;
            StringBuilder sbMenu = new StringBuilder();
            sb.AppendLine($"<select id =\"{id}\" name=\"{id}\" class=\"{className}\" style =\"{style}\">");
            foreach (var listItem in items)
            {
                var selected = string.Empty;
                if (selectedItem != null && listItem.Value == selectedItem.ToString())
                {
                    selected = " selected ";
                }

                sb.AppendLine($"<option value=\"{listItem.Value}\"{selected} >{listItem.Text}</option>");
            }

            sb.AppendLine("</select>");
            return new HtmlString(sb.ToString());
        }

        public static HtmlString BootstrapPager(this IHtmlHelper helper, int currentPageIndex, string url, long totalItems, long pageSize = 10, int numberOfLinks = 5)
        {
            if (totalItems <= 0)
            {
                return new HtmlString(string.Empty);
            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var lastPageNumber = (int)Math.Ceiling((double)currentPageIndex / numberOfLinks) * numberOfLinks;
            var firstPageNumber = lastPageNumber - (numberOfLinks - 1);
            var hasPreviousPage = currentPageIndex > 1;
            var hasNextPage = currentPageIndex < totalPages;
            if (lastPageNumber > totalPages)
            {
                lastPageNumber = totalPages;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<ul class = 'pagination pagination-sm no-margin pull-right'> ");

            sb.AppendLine(" " + AddLink(1, url, currentPageIndex == 1, "disabled", "&lt;&lt;", "First Page"));
            sb.AppendLine(" " + AddLink(currentPageIndex - 1, url, !hasPreviousPage, "disabled", "&lt;", "Previous Page"));
            for (int i = firstPageNumber; i <= lastPageNumber; i++)
            {
                sb.AppendLine(" " + AddLink(i, url, i == currentPageIndex, "active", i.ToString(), i.ToString()));
            }

            sb.AppendLine(" " + AddLink(currentPageIndex + 1, url, !hasNextPage, "disabled", "&gt;", "Next Page"));
            sb.AppendLine(" " + AddLink(totalPages, url, currentPageIndex == totalPages, "disabled", "&gt;&gt;", "Last Page"));

            sb.AppendLine(" </ul>");
            return new HtmlString(sb.ToString());
        }

        private static string AddLink(int index, string url, bool condition, string classToAdd, string linkText, string tooltip)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<li title='{0}'", tooltip);

            if (condition)
            {
                sb.AppendFormat(" class = '{0}'", classToAdd);
            }

            sb.AppendFormat("> ");

            var pageNumString = string.Empty;
            if (!condition)
            {
                if (url.Contains('?') == false)
                    pageNumString += "?Page=" + index;
                else
                    pageNumString += "&Page=" + index;
            }

            sb.AppendFormat("<a href='{0}{1}'>{2}</a> ", !condition ? url : "javascript:", pageNumString, linkText);

            sb.AppendFormat("</li> ");
            return sb.ToString();
        }
    }
}
