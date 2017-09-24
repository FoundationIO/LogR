using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Messages
{
    public static class NotifyControlHelper
    {
        public static void ShowErrorMsg(this Controller controller, string errorMsg)
        {
            controller.TempData[NotifyMessageContants.ErrorMsg] = errorMsg;
        }

        public static void ShowInfoMsg(this Controller controller, string errorMsg)
        {
            controller.TempData[NotifyMessageContants.InfoMsg] = errorMsg;
        }

        public static void ShowWarningMsg(this Controller controller, string errorMsg)
        {
            controller.TempData[NotifyMessageContants.WarningMsg] = errorMsg;
        }

        public static void ShowSuccessMsg(this Controller controller, string errorMsg)
        {
            controller.TempData[NotifyMessageContants.SuccessMsg] = errorMsg;
        }

        public static void ShowErrorMsgWithCaption(this Controller controller, string errorMsg)
        {
            controller.TempData[NotifyMessageContants.ShowCaption] = true;
            ShowErrorMsg(controller, errorMsg);
        }

        public static void ShowInfoMsgWithCaption(this Controller controller, string errorMsg)
        {
            controller.TempData[NotifyMessageContants.ShowCaption] = true;
            ShowInfoMsg(controller, errorMsg);
        }

        public static void ShowWarningMsgWithCaption(this Controller controller, string errorMsg)
        {
            controller.TempData[NotifyMessageContants.ShowCaption] = true;
            ShowWarningMsg(controller, errorMsg);
        }

        public static void ShowSuccessMsgWithCaption(this Controller controller, string errorMsg)
        {
            controller.TempData[NotifyMessageContants.ShowCaption] = true;
            ShowSuccessMsg(controller, errorMsg);
        }
    }
}