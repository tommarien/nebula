using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace Nebula.Data
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
                                                      cfg.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                                                  });

            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            SchemaMetadataUpdater.QuoteTableAndColumns(configuration);

            return configuration;
        }
    }
}