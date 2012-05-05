using FluentMigrator.VersionTableInfo;

namespace Nebula.Migrations
{
    [VersionTableMetaData]
    public class NebulaVersionMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get { return "System"; }
        }

        public string TableName
        {
            get { return "SchemaVersionInfo"; }
        }

        public string ColumnName
        {
            get { return "Version"; }
        }
    }
}