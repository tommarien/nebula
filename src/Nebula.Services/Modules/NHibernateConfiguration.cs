using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace Nebula.Services.Modules
{
    public static class NHibernateConfiguration
    {
        public static Configuration Build()
        {
            var configuration = new Configuration();

            // initialize mappers
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof (NHibernateConfiguration).Assembly.GetExportedTypes());

            // initialize database configuration
            configuration.DataBaseIntegration(cfg =>
                                                  {
                                                      cfg.ConnectionStringName = "Nebula";
                                                      cfg.Driver<Sql2008ClientDriver>();
                                                      cfg.Dialect<MsSql2008Dialect>();
#if DEBUG
                                                      cfg.LogSqlInConsole = true;
                                                      cfg.LogFormattedSql = true;
#endif
                                                      cfg.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                                                      cfg.SchemaAction = SchemaAutoAction.Create;
                                                  });
            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            SchemaMetadataUpdater.QuoteTableAndColumns(configuration);
            return configuration;
        }
    }
}