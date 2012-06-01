using FluentMigrator;

namespace Nebula.Migrations
{
    [Migration(2)]
    public class M0002_Add_Account_LastLogonDate : Migration
    {
        public override void Up()
        {
            Alter.Table(M0001_Add_Registration_Account.TableName)
                .InSchema(M0001_Add_Registration_Account.SchemaName)
                .AddColumn("LastLogOnDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("LastLogOnDate")
                .FromTable(M0001_Add_Registration_Account.TableName)
                .InSchema(M0001_Add_Registration_Account.SchemaName);
        }
    }
}