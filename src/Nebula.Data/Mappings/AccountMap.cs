using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Nebula.Domain.Registration;

namespace Nebula.Data.Mappings
{
    public class AccountMap : ClassMapping<Account>
    {
        public AccountMap()
        {
            Schema("Registration");

            DynamicUpdate(true);

            Id(ua => ua.Id, m => m.Generator(Generators.Identity));

            Property(ua => ua.UserName, m =>
                                            {
                                                m.Type(NHibernateUtil.AnsiString);
                                                m.Length(20);
                                                m.NotNullable(true);
                                                m.Unique(true);
                                            });

            Property(ua => ua.Password, m =>
                                            {
                                                m.Type(NHibernateUtil.AnsiString);
                                                m.Length(20);
                                                m.NotNullable(true);
                                            });

            Property(ua => ua.LastLogOnDate);
        }
    }
}