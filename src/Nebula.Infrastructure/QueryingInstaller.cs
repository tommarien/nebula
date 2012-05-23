using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.Infrastructure.Querying;

namespace Nebula.Infrastructure
{
    public class QueryingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacilityIfNeeded<TypedFactoryFacility>();

            container.Register(Component.For<IQueryFactory>().AsFactory());
            container.Register(Component.For<IQueryExecutor>().ImplementedBy<QueryExecutor>());
        }
    }
}