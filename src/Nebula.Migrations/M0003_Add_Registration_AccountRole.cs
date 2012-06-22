using FluentMigrator;

namespace Nebula.Migrations
{
    [Migration(3)]
    public class M0003_Add_Registration_AccountRole : Migration
    {
        public const string TableName = "AccountRole";

        public override void Up()
        {
            Create.Table(TableName).InSchema(M0001_Add_Registration_Account.SchemaName)
                .WithColumn("AccountId").AsInt32().NotNullable().Indexed()
                .WithColumn("RoleId").AsInt32().NotNullable().Indexed();

            Create.UniqueConstraint()
                .OnTable(TableName).WithSchema(M0001_Add_Registration_Account.SchemaName)
                .Columns(new[] {"AccountId", "RoleId"});

            Create.ForeignKey()
                .FromTable(TableName).InSchema(M0001_Add_Registration_Account.SchemaName)
                .ForeignColumn("AccountId")
                .ToTable(M0001_Add_Registration_Account.TableName).InSchema(M0001_Add_Registration_Account.SchemaName)
                .PrimaryColumn("Id");

            Create.ForeignKey()
                .FromTable(TableName).InSchema(M0001_Add_Registration_Account.SchemaName)
                .ForeignColumn("RoleId")
                .ToTable(M0002_Add_Registration_Role.TableName).InSchema(M0001_Add_Registration_Account.SchemaName)
                .PrimaryColumn("Id");

            Insert.IntoTable(TableName).InSchema(M0001_Add_Registration_Account.SchemaName)
                .Row(new {AccountId = 1, RoleId = 1});
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(M0001_Add_Registration_Account.SchemaName);
        }
    }
}