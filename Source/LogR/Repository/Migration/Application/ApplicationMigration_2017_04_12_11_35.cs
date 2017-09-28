using FluentMigrator;
using System;

namespace LogR.Repository.Migration.Application
{
    [Migration(201704121135)]
    public class ApplicationMigration_2017_04_12_11_35 : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("AppLog")
                .WithColumn("AppLogID").AsInt64().PrimaryKey().Identity()
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
                .WithColumn("AssignedTo").AsString().NotNullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

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
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                });
        }

        public override void Down()
        {
        }
    }
}
