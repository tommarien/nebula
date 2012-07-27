using FluentMigrator;

namespace Nebula.Migrations
{
    [Migration(5)]
    public class M0005_Alter_System_Log_Add_Index_Date : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Index()
                .OnTable(M0004_Add_System_Log.TableName).InSchema(M0004_Add_System_Log.SchemaName)
                .OnColumn("Date");
        }
    }
}