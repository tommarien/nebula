using System.Linq;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Infrastructure.Querying;
using Nebula.Infrastructure.Tests.Commanding;
using Nebula.Infrastructure.Tests.Querying;

namespace Nebula.Infrastructure.Tests
{
    [TestFixture]
    public class WindsorContainerExtensionsTests
    {
        [Test]
        public void AddFacilityIfNeeded_Should_add_the_facility_if_its_not_present()
        {
            var container = new WindsorContainer();

            container.AddFacilityIfNeeded<TypedFactoryFacility>();

            Assert.IsTrue(container.Kernel.GetFacilities().Any(f => f.GetType() == typeof (TypedFactoryFacility)));
        }

        [Test]
        public void AddFacilityIfNeeded_Should_not_add_the_facility_if_its_not_present()
        {
            var container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();

            Assert.DoesNotThrow(container.AddFacilityIfNeeded<TypedFactoryFacility>);
        }

        [Test]
        public void RegisterCommandHandlers_Should_register_all_commandHandlers()
        {
            var container = new WindsorContainer();

            container.RegisterCommandHandlers(typeof (TestCommand).Assembly);

            Assert.IsTrue(container.Kernel.HasComponent(typeof (ICommandHandler<TestCommand>)));
            Assert.IsTrue(container.Kernel.HasComponent(typeof (ICommandHandler<TestCommandWithResult, OperationResult>)));
        }

        [Test]
        public void RegisterQueries_Should_register_all_queries()
        {
            var container = new WindsorContainer();

            container.RegisterQueries(typeof (TestCommand).Assembly);

            Assert.IsTrue(container.Kernel.HasComponent(typeof (IQuery<int, MyPrecious>)));
        }
    }
}