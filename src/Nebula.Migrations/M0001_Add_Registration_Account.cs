using FluentMigrator;

namespace Nebula.Migrations
{
    [Migration(1)]
    public class M0001_Add_Registration_Account : Migration
    {
        public const string SchemaName = "Registration";
        public const string TableName = "Account";

        public override void Up()
        {
            Create.Schema(SchemaName);

            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                .WithColumn("UserName").AsAnsiString(20).NotNullable().Unique()
                .WithColumn("Hash").AsAnsiString(44).NotNullable()
                .WithColumn("Salt").AsAnsiString(28).NotNullable()
                .WithColumn("LastLogOnDate").AsDateTime().Nullable();

            Insert.IntoTable(TableName).InSchema(SchemaName)
                .Row(new
                         {
                             UserName = "admin",
                             Hash = "sxg8SwG3XptWVtWb7vXURUVVhLmkI5S7t2eusUqSOH0=",
                             Salt = "ZUzwGoB4PyY5zC0TlDv0VqZIsj0="
                         });
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(SchemaName);

            Delete.Schema(SchemaName);
        }
    }
}