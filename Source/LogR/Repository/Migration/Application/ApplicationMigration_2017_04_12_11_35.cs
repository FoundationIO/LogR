using System;
using FluentMigrator;

namespace LogR.Repository.Migration.Application
{
    [Migration(201704121135)]
    [Profile("AppStore")]
    public class ApplicationMigration_2017_04_12_11_35 : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("UserID").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserName").AsString(50).NotNullable().Indexed("Idx_User_UserName")
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("FirstName").AsString(100).NotNullable()
                .WithColumn("LastName").AsString(100).NotNullable()
                .WithColumn("Email").AsString(200).NotNullable()
                .WithColumn("AllowAdminOperations").AsBoolean().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

            Create.Table("Configuration")
                .WithColumn("ConfigurationID").AsInt64().PrimaryKey().Identity()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

            Create.Table("AccessKey")
                .WithColumn("AccessKeyID").AsInt64().PrimaryKey().Identity()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("AssignedTo").AsString().NotNullable()
                .WithColumn("Notes").AsString().Nullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

            Create.Table("LogType")
                .WithColumn("LogTypeId").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsBoolean().NotNullable()
                .WithColumn("ErrorSeverityValue").AsString().NotNullable()
                .WithColumn("WarningSeverityValue").AsString().NotNullable()
                .WithColumn("SqlErrorSeverityValue").AsString().NotNullable()
                .WithColumn("CanShowLogIdInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowLogTypeInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowApplicationIdInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowCorelationIdInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowFunctionIdInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowLongdateInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowReceivedDateInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowSeverityInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowAppInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowModuleInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowMachineNameInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowFunctionNameInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowProcessIdInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowThreadIdInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowCurrentFunctionInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowCurrentSourceFilenameInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowCurrentSourceLineNumberInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowCurrentTagInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowUserIdentityInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowRemoteAddressInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowUserAgentInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowResultInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowResultCodeInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowMessageInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowStartTimeInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowElapsedTimeInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowRequestInSearchBox").AsBoolean().NotNullable()
                .WithColumn("CanShowResponseInSearchBox").AsBoolean().NotNullable()
                //Can show in List view - as none or main column or child column
                .WithColumn("ShowLogIdInList").AsInt32().NotNullable()
                .WithColumn("ShowLogTypeInList").AsInt32().NotNullable()
                .WithColumn("ShowApplicationIdInList").AsInt32().NotNullable()
                .WithColumn("ShowCorelationIdInList").AsInt32().NotNullable()
                .WithColumn("ShowFunctionIdInList").AsInt32().NotNullable()
                .WithColumn("ShowLongdateInList").AsInt32().NotNullable()
                .WithColumn("ShowReceivedDateInList").AsInt32().NotNullable()
                .WithColumn("ShowSeverityInList").AsInt32().NotNullable()
                .WithColumn("ShowAppInList").AsInt32().NotNullable()
                .WithColumn("ShowModuleInList").AsInt32().NotNullable()
                .WithColumn("ShowMachineNameInList").AsInt32().NotNullable()
                .WithColumn("ShowFunctionNameInList").AsInt32().NotNullable()
                .WithColumn("ShowProcessIdInList").AsInt32().NotNullable()
                .WithColumn("ShowThreadIdInList").AsInt32().NotNullable()
                .WithColumn("ShowCurrentFunctionInList").AsInt32().NotNullable()
                .WithColumn("ShowCurrentSourceFilenameInList").AsInt32().NotNullable()
                .WithColumn("ShowCurrentSourceLineNumberInList").AsInt32().NotNullable()
                .WithColumn("ShowCurrentTagInList").AsInt32().NotNullable()
                .WithColumn("ShowUserIdentityInList").AsInt32().NotNullable()
                .WithColumn("ShowRemoteAddressInList").AsInt32().NotNullable()
                .WithColumn("ShowUserAgentInList").AsInt32().NotNullable()
                .WithColumn("ShowResultInList").AsInt32().NotNullable()
                .WithColumn("ShowResultCodeInList").AsInt32().NotNullable()
                .WithColumn("ShowMessageInList").AsInt32().NotNullable()
                .WithColumn("ShowStartTimeInList").AsInt32().NotNullable()
                .WithColumn("ShowElapsedTimeInList").AsInt32().NotNullable()
                .WithColumn("ShowRequestInList").AsInt32().NotNullable()
                .WithColumn("ShowResponseInList").AsInt32().NotNullable();

            Insert.IntoTable("User")
                .Row(new
                {
                    UserName = "root",
                    Password = "root",
                    FirstName = "Root",
                    LastName = "Rooter",
                    Email = "root@localhost",
                    AllowAdminOperations = true,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                });

            Insert.IntoTable("LogType")
                .Row(new
                    {
                        LogTypeId = 1,
                        Name = "AppLog",
                        ErrorSeverityValue = "ERROR",
                        WarningSeverityValue = "WARNING",
                        SqlErrorSeverityValue = "SQLERROR",
                        //Can show in Visual Search
                        CanShowLogIdInSearchBox = true,
                        CanShowLogTypeInSearchBox = true,
                        CanShowApplicationIdInSearchBox = true,
                        CanShowCorelationIdInSearchBox = true,
                        CanShowFunctionIdInSearchBox = true,
                        CanShowLongdateInSearchBox = true,
                        CanShowReceivedDateInSearchBox = true,
                        CanShowSeverityInSearchBox = true,
                        CanShowAppInSearchBox = true,
                        CanShowModuleInSearchBox = true,
                        CanShowMachineNameInSearchBox = true,
                        CanShowFunctionNameInSearchBox = true,
                        CanShowProcessIdInSearchBox = true,
                        CanShowThreadIdInSearchBox = true,
                        CanShowCurrentFunctionInSearchBox = true,
                        CanShowCurrentSourceFilenameInSearchBox = true,
                        CanShowCurrentSourceLineNumberInSearchBox = true,
                        CanShowCurrentTagInSearchBox = true,
                        CanShowUserIdentityInSearchBox = true,
                        CanShowRemoteAddressInSearchBox = true,
                        CanShowUserAgentInSearchBox = true,
                        CanShowResultInSearchBox = true,
                        CanShowResultCodeInSearchBox = true,
                        CanShowMessageInSearchBox = true,
                        CanShowStartTimeInSearchBox = true,
                        CanShowElapsedTimeInSearchBox = true,
                        CanShowRequestInSearchBox = true,
                        CanShowResponseInSearchBox = true,
                        //Can show in List view - as none or main column or child column
                        ShowLogIdInList = 1,
                        ShowLogTypeInList = 1,
                        ShowApplicationIdInList = 1,
                        ShowCorelationIdInList = 1,
                        ShowFunctionIdInList = 1,
                        ShowLongdateInList = 1,
                        ShowReceivedDateInList = 1,
                        ShowSeverityInList = 1,
                        ShowAppInList = 1,
                        ShowModuleInList = 1,
                        ShowMachineNameInList = 1,
                        ShowFunctionNameInList = 1,
                        ShowProcessIdInList = 1,
                        ShowThreadIdInList = 1,
                        ShowCurrentFunctionInList = 1,
                        ShowCurrentSourceFilenameInList = 1,
                        ShowCurrentSourceLineNumberInList = 1,
                        ShowCurrentTagInList = 1,
                        ShowUserIdentityInList = 1,
                        ShowRemoteAddressInList = 1,
                        ShowUserAgentInList = 1,
                        ShowResultInList = 1,
                        ShowResultCodeInList = 1,
                        ShowMessageInList = 1,
                        ShowStartTimeInList = 1,
                        ShowElapsedTimeInList = 1,
                        ShowRequestInList = 1,
                        ShowResponseInList = 1
                    });

            Insert.IntoTable("LogType")
            .Row(new
            {
                LogTypeId = 1,
                Name = "PerformanceLog",
                ErrorSeverityValue = "ERROR",
                WarningSeverityValue = "WARNING",
                SqlErrorSeverityValue = "SQLERROR",
                //Can show in Visual Search
                CanShowLogIdInSearchBox = true,
                CanShowLogTypeInSearchBox = true,
                CanShowApplicationIdInSearchBox = true,
                CanShowCorelationIdInSearchBox = true,
                CanShowFunctionIdInSearchBox = true,
                CanShowLongdateInSearchBox = true,
                CanShowReceivedDateInSearchBox = true,
                CanShowSeverityInSearchBox = true,
                CanShowAppInSearchBox = true,
                CanShowModuleInSearchBox = true,
                CanShowMachineNameInSearchBox = true,
                CanShowFunctionNameInSearchBox = true,
                CanShowProcessIdInSearchBox = true,
                CanShowThreadIdInSearchBox = true,
                CanShowCurrentFunctionInSearchBox = true,
                CanShowCurrentSourceFilenameInSearchBox = true,
                CanShowCurrentSourceLineNumberInSearchBox = true,
                CanShowCurrentTagInSearchBox = true,
                CanShowUserIdentityInSearchBox = true,
                CanShowRemoteAddressInSearchBox = true,
                CanShowUserAgentInSearchBox = true,
                CanShowResultInSearchBox = true,
                CanShowResultCodeInSearchBox = true,
                CanShowMessageInSearchBox = true,
                CanShowStartTimeInSearchBox = true,
                CanShowElapsedTimeInSearchBox = true,
                CanShowRequestInSearchBox = true,
                CanShowResponseInSearchBox = true,
                //Can show in List view - as none or main column or child column
                ShowLogIdInList = 1,
                ShowLogTypeInList = 1,
                ShowApplicationIdInList = 1,
                ShowCorelationIdInList = 1,
                ShowFunctionIdInList = 1,
                ShowLongdateInList = 1,
                ShowReceivedDateInList = 1,
                ShowSeverityInList = 1,
                ShowAppInList = 1,
                ShowModuleInList = 1,
                ShowMachineNameInList = 1,
                ShowFunctionNameInList = 1,
                ShowProcessIdInList = 1,
                ShowThreadIdInList = 1,
                ShowCurrentFunctionInList = 1,
                ShowCurrentSourceFilenameInList = 1,
                ShowCurrentSourceLineNumberInList = 1,
                ShowCurrentTagInList = 1,
                ShowUserIdentityInList = 1,
                ShowRemoteAddressInList = 1,
                ShowUserAgentInList = 1,
                ShowResultInList = 1,
                ShowResultCodeInList = 1,
                ShowMessageInList = 1,
                ShowStartTimeInList = 1,
                ShowElapsedTimeInList = 1,
                ShowRequestInList = 1,
                ShowResponseInList = 1
            });

            Insert.IntoTable("LogType")
            .Row(new
            {
                LogTypeId = 3,
                Name = "EventLog",
                ErrorSeverityValue = "ERROR",
                WarningSeverityValue = "WARNING",
                SqlErrorSeverityValue = "SQLERROR",
                //Can show in Visual Search
                CanShowLogIdInSearchBox = true,
                CanShowLogTypeInSearchBox = true,
                CanShowApplicationIdInSearchBox = true,
                CanShowCorelationIdInSearchBox = true,
                CanShowFunctionIdInSearchBox = true,
                CanShowLongdateInSearchBox = true,
                CanShowReceivedDateInSearchBox = true,
                CanShowSeverityInSearchBox = true,
                CanShowAppInSearchBox = true,
                CanShowModuleInSearchBox = true,
                CanShowMachineNameInSearchBox = true,
                CanShowFunctionNameInSearchBox = true,
                CanShowProcessIdInSearchBox = true,
                CanShowThreadIdInSearchBox = true,
                CanShowCurrentFunctionInSearchBox = true,
                CanShowCurrentSourceFilenameInSearchBox = true,
                CanShowCurrentSourceLineNumberInSearchBox = true,
                CanShowCurrentTagInSearchBox = true,
                CanShowUserIdentityInSearchBox = true,
                CanShowRemoteAddressInSearchBox = true,
                CanShowUserAgentInSearchBox = true,
                CanShowResultInSearchBox = true,
                CanShowResultCodeInSearchBox = true,
                CanShowMessageInSearchBox = true,
                CanShowStartTimeInSearchBox = true,
                CanShowElapsedTimeInSearchBox = true,
                CanShowRequestInSearchBox = true,
                CanShowResponseInSearchBox = true,
                //Can show in List view - as none or main column or child column
                ShowLogIdInList = 1,
                ShowLogTypeInList = 1,
                ShowApplicationIdInList = 1,
                ShowCorelationIdInList = 1,
                ShowFunctionIdInList = 1,
                ShowLongdateInList = 1,
                ShowReceivedDateInList = 1,
                ShowSeverityInList = 1,
                ShowAppInList = 1,
                ShowModuleInList = 1,
                ShowMachineNameInList = 1,
                ShowFunctionNameInList = 1,
                ShowProcessIdInList = 1,
                ShowThreadIdInList = 1,
                ShowCurrentFunctionInList = 1,
                ShowCurrentSourceFilenameInList = 1,
                ShowCurrentSourceLineNumberInList = 1,
                ShowCurrentTagInList = 1,
                ShowUserIdentityInList = 1,
                ShowRemoteAddressInList = 1,
                ShowUserAgentInList = 1,
                ShowResultInList = 1,
                ShowResultCodeInList = 1,
                ShowMessageInList = 1,
                ShowStartTimeInList = 1,
                ShowElapsedTimeInList = 1,
                ShowRequestInList = 1,
                ShowResponseInList = 1
            });

            Insert.IntoTable("LogType")
            .Row(new
            {
                LogTypeId = 3,
                Name = "WebLog",
                ErrorSeverityValue = "ERROR",
                WarningSeverityValue = "WARNING",
                SqlErrorSeverityValue = "SQLERROR",
                //Can show in Visual Search
                CanShowLogIdInSearchBox = true,
                CanShowLogTypeInSearchBox = true,
                CanShowApplicationIdInSearchBox = true,
                CanShowCorelationIdInSearchBox = true,
                CanShowFunctionIdInSearchBox = true,
                CanShowLongdateInSearchBox = true,
                CanShowReceivedDateInSearchBox = true,
                CanShowSeverityInSearchBox = true,
                CanShowAppInSearchBox = true,
                CanShowModuleInSearchBox = true,
                CanShowMachineNameInSearchBox = true,
                CanShowFunctionNameInSearchBox = true,
                CanShowProcessIdInSearchBox = true,
                CanShowThreadIdInSearchBox = true,
                CanShowCurrentFunctionInSearchBox = true,
                CanShowCurrentSourceFilenameInSearchBox = true,
                CanShowCurrentSourceLineNumberInSearchBox = true,
                CanShowCurrentTagInSearchBox = true,
                CanShowUserIdentityInSearchBox = true,
                CanShowRemoteAddressInSearchBox = true,
                CanShowUserAgentInSearchBox = true,
                CanShowResultInSearchBox = true,
                CanShowResultCodeInSearchBox = true,
                CanShowMessageInSearchBox = true,
                CanShowStartTimeInSearchBox = true,
                CanShowElapsedTimeInSearchBox = true,
                CanShowRequestInSearchBox = true,
                CanShowResponseInSearchBox = true,
                //Can show in List view - as none or main column or child column
                ShowLogIdInList = 1,
                ShowLogTypeInList = 1,
                ShowApplicationIdInList = 1,
                ShowCorelationIdInList = 1,
                ShowFunctionIdInList = 1,
                ShowLongdateInList = 1,
                ShowReceivedDateInList = 1,
                ShowSeverityInList = 1,
                ShowAppInList = 1,
                ShowModuleInList = 1,
                ShowMachineNameInList = 1,
                ShowFunctionNameInList = 1,
                ShowProcessIdInList = 1,
                ShowThreadIdInList = 1,
                ShowCurrentFunctionInList = 1,
                ShowCurrentSourceFilenameInList = 1,
                ShowCurrentSourceLineNumberInList = 1,
                ShowCurrentTagInList = 1,
                ShowUserIdentityInList = 1,
                ShowRemoteAddressInList = 1,
                ShowUserAgentInList = 1,
                ShowResultInList = 1,
                ShowResultCodeInList = 1,
                ShowMessageInList = 1,
                ShowStartTimeInList = 1,
                ShowElapsedTimeInList = 1,
                ShowRequestInList = 1,
                ShowResponseInList = 1
            });

            Insert.IntoTable("Configuration")
                .Row(new
                {
                    Key = "EnableAccessKey",
                    Value = "false",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                });

            Insert.IntoTable("AccessKey")
                .Row(new
                {
                    Key = "205e9001-bcbb-4321-b3ab-c6ec7e9a707b",
                    AssignedTo = "All",
                    Notes = "Auto Generated Key",
                    Active = false,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                });
        }

        public override void Down()
        {
        }
    }
}