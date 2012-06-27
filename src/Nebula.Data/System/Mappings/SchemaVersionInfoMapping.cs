using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using Nebula.Domain.System;

namespace Nebula.Data.System.Mappings
{
    public class SchemaVersionInfoMapping : ClassMapping<SchemaVersionInfo>
    {
        public SchemaVersionInfoMapping()
        {
            Schema("System");

            Mutable(false);

            Id(v => v.Version);
            Property(v => v.AppliedOn, m => m.Type(NHibernateUtil.UtcDateTime));
        }
    }
}