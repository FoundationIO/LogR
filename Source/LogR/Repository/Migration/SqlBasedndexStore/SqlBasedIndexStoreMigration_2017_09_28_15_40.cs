using FluentMigrator;

namespace LogR.Repository.Migration.Application
{
    [Migration(201709281540)]
    [Profile("LogIndexStore")]
    public class SqlBasedIndexStoreMigration_2017_09_28_15_40 : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("AppLog")
                .WithColumn("AppLogId").AsInt64().PrimaryKey().Identity()
                .WithColumn("LogType").AsInt32()
                .WithColumn("CorelationId").AsString()
                .WithColumn("Longdate").AsDateTime()
                .WithColumn("LongdateAsTicks").AsInt64()
                .WithColumn("Severity").AsString()
                .WithColumn("App").AsString()
                .WithColumn("MachineName").AsString()
                .WithColumn("ProcessId").AsString()
                .WithColumn("ThreadId").AsString()
                .WithColumn("CurrentTag").AsString()
                .WithColumn("CurrentFunction").AsString()
                .WithColumn("CurrentSourceFilename").AsString()
                .WithColumn("CurrentSourceLineNumber").AsString()
                .WithColumn("UserIdentity").AsString()
                .WithColumn("RemoteAddress").AsString()
                .WithColumn("UserAgent").AsString()
                .WithColumn("Module").AsString()
                .WithColumn("FunctionName").AsString()
                .WithColumn("Result").AsString()
                .WithColumn("StartTime").AsString()
                .WithColumn("ElapsedTime").AsString()
                .WithColumn("Status").AsString()
                .WithColumn("Request").AsString()
                .WithColumn("Response").AsString();

            Create.Table("AppLogAdditionalProperty")
                .WithColumn("AppLogAdditionalPropertyId").AsInt64().PrimaryKey().Identity()
                .WithColumn("AppLogId").AsInt64().ForeignKey("AppLog", "AppLogId")
                .WithColumn("Key").AsString()
                .WithColumn("Value").AsString();
        }

        public override void Down()
        {
        }
    }
}
