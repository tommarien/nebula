using FluentMigrator;

namespace Nebula.Migrations
{
    [Migration(1)]
    public class M0001_Add_Registration_UserAccount : Migration
    {
        public const string SchemaName = "Registration";
        public const string TableName = "UserAccount";

        public override void Up()
        {
            Create.Schema(SchemaName);

            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                .WithColumn("UserName").AsAnsiString(20).NotNullable().Unique()
                .WithColumn("Password").AsAnsiString(20).NotNullable();

            Insert.IntoTable(TableName).InSchema(SchemaName).Row(new {UserName = "admin", Password = "admin"});
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(SchemaName);

            Delete.Schema(SchemaName);
        }
    }
}