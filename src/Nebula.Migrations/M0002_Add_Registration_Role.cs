using FluentMigrator;

namespace Nebula.Migrations
{
    [Migration(2)]
    public class M0002_Add_Registration_Role : Migration
    {
        public const string TableName = "Role";

        public override void Up()
        {
            Create.Table(TableName).InSchema(M0001_Add_Registration_Account.SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("Name").AsAnsiString(40).NotNullable();

            Insert.IntoTable(TableName).InSchema(M0001_Add_Registration_Account.SchemaName)
                .Row(new {Id = 1, Name = "Administrator"});
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(M0001_Add_Registration_Account.SchemaName);
        }
    }
}