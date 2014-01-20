using FluentMigrator.VersionTableInfo;

namespace Nebula.Migrations
{
    [VersionTableMetaData]
    public class NebulaVersionMetaData : DefaultVersionTableMetaData
    {
        public override string SchemaName
        {
            get { return "System"; }
        }

        public override string TableName
        {
            get { return "SchemaVersionInfo"; }
        }
    }
}