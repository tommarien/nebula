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

            Id(account => account.Id, m => m.Generator(Generators.Identity));

            Property(account => account.UserName, m =>
                                                      {
                                                          m.Type(NHibernateUtil.AnsiString);
                                                          m.Length(20);
                                                          m.NotNullable(true);
                                                          m.Unique(true);
                                                      });

            Component(account => account.Password, m => m.Property(p => p.Value, p =>
                                                                                     {
                                                                                         p.Type(NHibernateUtil.AnsiString);
                                                                                         p.Column("Password");
                                                                                         p.Access(Accessor.NoSetter);
                                                                                         p.Length(20);
                                                                                         p.NotNullable(true);
                                                                                     }));

            Property(account => account.LastLogOnDate);
        }
    }
}