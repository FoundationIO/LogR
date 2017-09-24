using Microsoft.AspNetCore.Html;

namespace Framework.Web.Messages
{
    public static class NotifyHtmlHelper
    {
        public static HtmlString DisplayMsg(string msgType, string msg, bool canHide)
        {
            return DisplayMsg(msgType, "Error", msg, canHide);
        }

        public static HtmlString DisplayMsg(string msgType, string caption, string msg, bool canHide)
        {
            return new HtmlString(string.Format("<p class=\"flash {0}\"> {1} </p>", msgType, msg));
        }

        public static HtmlString DisplayInfoMsg(string caption, string infoMsg, bool canHide)
        {
            return DisplayMsg("information", caption, infoMsg, canHide);
        }

        public static HtmlString DisplayInfoMsg(string caption, string infoMsg)
        {
            return DisplayInfoMsg(caption, infoMsg, true);
        }

        public static HtmlString DisplayInfoMsg(string infoMsg, bool canHide)
        {
            return DisplayInfoMsg("Information", infoMsg, canHide);
        }

        public static HtmlString DisplayInfoMsg(string infoMsg)
        {
            return DisplayInfoMsg(infoMsg, true);
        }

        public static HtmlString DisplayErrorMsg(string caption, string infoMsg, bool canHide)
        {
            return DisplayMsg("failure", caption, infoMsg, canHide);
        }

        public static HtmlString DisplayErrorMsg(string caption, string infoMsg)
        {
            return DisplayErrorMsg(caption, infoMsg, true);
        }

        public static HtmlString DisplayErrorMsg(string infoMsg, bool canHide)
        {
            return DisplayErrorMsg("Error", infoMsg, canHide);
        }

        public static HtmlString DisplayErrorMsg(string infoMsg)
        {
            return DisplayErrorMsg(infoMsg, true);
        }

        public static HtmlString DisplayWarningMsg(string caption, string infoMsg, bool canHide)
        {
            return DisplayMsg("warning", caption, infoMsg, canHide);
        }

        public static HtmlString DisplayWarningMsg(string caption, string infoMsg)
        {
            return DisplayWarningMsg(caption, infoMsg, true);
        }

        public static HtmlString DisplayWarningMsg(string infoMsg, bool canHide)
        {
            return DisplayWarningMsg("Warning", infoMsg, canHide);
        }

        public static HtmlString DisplayWarningMsg(string infoMsg)
        {
            return DisplayWarningMsg(infoMsg, true);
        }

        public static HtmlString DisplaySuccessMsg(string caption, string infoMsg, bool canHide)
        {
            return DisplayMsg("success", caption, infoMsg, canHide);
        }

        public static HtmlString DisplaySuccessMsg(string caption, string infoMsg)
        {
            return DisplaySuccessMsg(caption, infoMsg, true);
        }

        public static HtmlString DisplaySuccessMsg(string infoMsg, bool canHide)
        {
            return DisplaySuccessMsg("Success", infoMsg, canHide);
        }

        public static HtmlString DisplaySuccessMsg(string infoMsg)
        {
            return DisplaySuccessMsg(infoMsg, true);
        }
    }
}