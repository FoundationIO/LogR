using FluentMigrator;

namespace LogR.Repository.Migration.SqlBasedndexStore
{
    [Migration(201709281540)]
    [Profile("LogIndexStore")]
    public class SqlBasedIndexStoreMigration_2017_09_28_15_40 : FluentMigrator.Migration
    {
        private static bool AlreadyCalled { get; set; } = false;

        public override void Up()
        {
            //Fixme: Not sure why the migration is called twice here.
            //We need to have this fixed.
            if (AlreadyCalled == true)
                return;

            AlreadyCalled = true;

            Create.Table("AppLog")
                .WithColumn("AppLogId").AsInt64().PrimaryKey().Identity()
                .WithColumn("LogId").AsGuid()
                .WithColumn("LogType").AsInt32()
                .WithColumn("ApplicationId").AsString().Nullable()
                .WithColumn("CorelationId").AsString().Nullable()
                .WithColumn("FunctionId").AsString().Nullable()
                .WithColumn("ReceivedDate").AsDateTime().NotNullable()
                .WithColumn("ReceivedDateAsTicks").AsInt64().NotNullable()
                .WithColumn("Longdate").AsDateTime().NotNullable()
                .WithColumn("LongdateAsTicks").AsInt64().NotNullable()
                .WithColumn("Severity").AsString().Nullable()
                .WithColumn("App").AsString().Nullable()
                .WithColumn("Module").AsString().Nullable()
                .WithColumn("MachineName").AsString().Nullable()
                .WithColumn("FunctionName").AsString().Nullable()
                .WithColumn("ProcessId").AsString().Nullable()
                .WithColumn("ThreadId").AsString().Nullable()
                .WithColumn("CurrentFunction").AsString().Nullable()
                .WithColumn("CurrentSourceFilename").AsString().Nullable()
                .WithColumn("CurrentSourceLineNumber").AsString().Nullable()
                .WithColumn("CurrentTag").AsString().Nullable()
                .WithColumn("UserIdentity").AsString().Nullable()
                .WithColumn("RemoteAddress").AsString().Nullable()
                .WithColumn("UserAgent").AsString().Nullable()
                .WithColumn("Result").AsString().Nullable()
                .WithColumn("ResultCode").AsInt32().Nullable()
                .WithColumn("Message").AsString().Nullable()
                .WithColumn("StartTime").AsString().Nullable()
                .WithColumn("ElapsedTime").AsString().Nullable()
                .WithColumn("Status").AsString().Nullable()
                .WithColumn("Request").AsString().Nullable()
                .WithColumn("Response").AsString().Nullable();

            Create.Table("AppLogAdditionalProperty")
                .WithColumn("AppLogAdditionalPropertyId").AsInt64().PrimaryKey().Identity()
                .WithColumn("AppLogId").AsInt64().ForeignKey("AppLog", "AppLogId")
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("Value").AsString().Nullable();
        }

        public override void Down()
        {
        }
    }
}
