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
                                                          m.Update(false);
                                                          m.Type(NHibernateUtil.AnsiString);
                                                          m.Length(20);
                                                      });

            Component(account => account.Password, m =>
                                                       {
                                                           m.Property("hash", p =>
                                                                                  {
                                                                                      p.Column("Hash");
                                                                                      p.Type(NHibernateUtil.AnsiString);
                                                                                      p.Access(Accessor.Field);
                                                                                      p.Length(44);
                                                                                  });
                                                           m.Property("salt", p =>
                                                                                  {
                                                                                      p.Column("Salt");
                                                                                      p.Type(NHibernateUtil.AnsiString);
                                                                                      p.Access(Accessor.Field);
                                                                                      p.Length(28);
                                                                                  });
                                                       });

            Property(account => account.LastLogOnDate);
        }
    }
}