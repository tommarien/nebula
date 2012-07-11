using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Nebula.Domain.System;

namespace Nebula.Data.System.Mappings
{
    public class LogMapping : ClassMapping<Log>
    {
        public LogMapping()
        {
            Schema("System");
            Mutable(false);

            Id(l => l.Id, m => m.Generator(Generators.Identity));
            Property(l => l.Date);
            Property(l => l.Level, m =>
                                       {
                                           m.Type(NHibernateUtil.AnsiString);
                                           m.Length(50);
                                       });
            Property(l => l.Logger, m =>
                                        {
                                            m.Type(NHibernateUtil.AnsiString);
                                            m.Length(255);
                                        });
            Property(l => l.Message, m =>
                                         {
                                             m.Type(NHibernateUtil.AnsiString);
                                             m.Length(2000);
                                         });
            Property(l => l.Exception, m =>
                                           {
                                               m.Type(NHibernateUtil.AnsiString);
                                               m.Length(4000);
                                           });
            Property(l => l.HostName, m =>
                                          {
                                              m.Type(NHibernateUtil.AnsiString);
                                              m.Length(255);
                                          });
            Property(l => l.SessionId, m =>
                                           {
                                               m.Type(NHibernateUtil.AnsiString);
                                               m.Length(255);
                                           });
            Property(l => l.UserName, m =>
                                          {
                                              m.Type(NHibernateUtil.AnsiString);
                                              m.Length(255);
                                          });
        }
    }
}