using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace Nebula.Data
{
    public class NHibernateConfigurationBuilder
    {
        private string connectionStringName;

        public NHibernateConfigurationBuilder()
        {
            connectionStringName = "Nebula";
        }

        public NHibernateConfigurationBuilder UsingNamedConnectionString(string connectionStringName)
        {
            this.connectionStringName = connectionStringName;
            return this;
        }

        public Configuration Build()
        {
            var configuration = new Configuration();

            // initialize database configuration
            configuration.DataBaseIntegration(cfg =>
            {
                cfg.ConnectionStringName = connectionStringName;
                cfg.Driver<Sql2008ClientDriver>();
                cfg.Dialect<MsSql2008Dialect>();
                cfg.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            });

            // initialize mappers
            var mapper = new ModelMapper();
            mapper.AddMappings(GetType().Assembly.GetExportedTypes());
            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            // Auto Quote all table and column names
            SchemaMetadataUpdater.QuoteTableAndColumns(configuration);


            return configuration;
        }
    }
}