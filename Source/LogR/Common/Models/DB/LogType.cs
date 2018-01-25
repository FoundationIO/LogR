namespace LogR.Common.Models.DB
{
    public class LogType
    {
        public long LogTypeId { get; set; }

        public string Name { get; set; }

        public string ErrorSeverityValue { get; set; }

        public string WarningSeverityValue { get; set; }

        public string SqlErrorSeverityValue { get; set; }

        //Can show in Visual Search
        public bool CanShowLogIdInSearchBox { get; set; }

        public bool CanShowLogTypeInSearchBox { get; set; }

        public bool CanShowApplicationIdInSearchBox { get; set; }

        public bool CanShowCorelationIdInSearchBox { get; set; }

        public bool CanShowFunctionIdInSearchBox { get; set; }

        public bool CanShowLongdateInSearchBox { get; set; }

        public bool CanShowReceivedDateInSearchBox { get; set; }

        public bool CanShowSeverityInSearchBox { get; set; }

        public bool CanShowAppInSearchBox { get; set; }

        public bool CanShowModuleInSearchBox { get; set; }

        public bool CanShowMachineNameInSearchBox { get; set; }

        public bool CanShowFunctionNameInSearchBox { get; set; }

        public bool CanShowProcessIdInSearchBox { get; set; }

        public bool CanShowThreadIdInSearchBox { get; set; }

        public bool CanShowCurrentFunctionInSearchBox { get; set; }

        public bool CanShowCurrentSourceFilenameInSearchBox { get; set; }

        public bool CanShowCurrentSourceLineNumberInSearchBox { get; set; }

        public bool CanShowCurrentTagInSearchBox { get; set; }

        public bool CanShowUserIdentityInSearchBox { get; set; }

        public bool CanShowRemoteAddressInSearchBox { get; set; }

        public bool CanShowUserAgentInSearchBox { get; set; }

        public bool CanShowResultInSearchBox { get; set; }

        public bool CanShowResultCodeInSearchBox { get; set; }

        public bool CanShowMessageInSearchBox { get; set; }

        public bool CanShowStartTimeInSearchBox { get; set; }

        public bool CanShowElapsedTimeInSearchBox { get; set; }

        public bool CanShowRequestInSearchBox { get; set; }

        public bool CanShowResponseInSearchBox { get; set; }

        //Can show in List view - as none or main column or child column
        public int ShowLogIdInList { get; set; }

        public int ShowLogTypeInList { get; set; }

        public int ShowApplicationIdInList { get; set; }

        public int ShowCorelationIdInList { get; set; }

        public int ShowFunctionIdInList { get; set; }

        public int ShowLongdateInList { get; set; }

        public int ShowReceivedDateInList { get; set; }

        public int ShowSeverityInList { get; set; }

        public int ShowAppInList { get; set; }

        public int ShowModuleInList { get; set; }

        public int ShowMachineNameInList { get; set; }

        public int ShowFunctionNameInList { get; set; }

        public int ShowProcessIdInList { get; set; }

        public int ShowThreadIdInList { get; set; }

        public int ShowCurrentFunctionInList { get; set; }

        public int ShowCurrentSourceFilenameInList { get; set; }

        public int ShowCurrentSourceLineNumberInList { get; set; }

        public int ShowCurrentTagInList { get; set; }

        public int ShowUserIdentityInList { get; set; }

        public int ShowRemoteAddressInList { get; set; }

        public int ShowUserAgentInList { get; set; }

        public int ShowResultInList { get; set; }

        public int ShowResultCodeInList { get; set; }

        public int ShowMessageInList { get; set; }

        public int ShowStartTimeInList { get; set; }

        public int ShowElapsedTimeInList { get; set; }

        public int ShowRequestInList { get; set; }

        public int ShowResponseInList { get; set; }
    }
}
