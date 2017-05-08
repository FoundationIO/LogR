using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogR.Repository.Migration
{
    [Migration(201704121135)]
    public class Migration_2017_04_12_11_35 : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("ID").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserName").AsString(50).NotNullable().Indexed("Idx_User_UserName")
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("FirstName").AsString(100).NotNullable()
                .WithColumn("LastName").AsString(100).NotNullable()
                .WithColumn("AllowAdminOperations").AsBoolean().NotNullable();

            Create.Table("FirstTimeConfiguration")
                .WithColumn("ID").AsInt64().PrimaryKey().Identity()
                .WithColumn("DoneDate").AsDateTime().Nullable()
                .WithColumn("Done").AsBoolean().WithDefaultValue(false);


            Insert.IntoTable("User")
                .Row(new { UserName = "root" , Password = "root" , FirstName = "Root" , LastName = "Rooter" , AllowAdminOperations = true });

        }

        public override void Down()
        {

        }
    }
}
