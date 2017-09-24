using System.IO;
using Microsoft.AspNetCore.Html;

namespace Framework.Web.HtmlHelpers
{
    public static class HtmlContentExtensions
    {
        public static string ToHtmlString(this IHtmlContent tag)
        {
            using (var writer = new StringWriter())
            {
                tag.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
