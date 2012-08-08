using NUnit.Framework;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Controllers;
using Nebula.MvcApplication.Models;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.System
{
    [TestFixture]
    public class SystemVersionGetTests : HttpContextFixture
    {
        private SystemController controller;
        private IQueryHandler<Query, long> query;

        protected override void AfterSetUp()
        {
            var queryFactory = MockRepository.GenerateStub<IQueryHandlerFactory>();
            query = MockRepository.GenerateMock<IQueryHandler<Query, long>>();

            queryFactory.Stub(f => f.CreateHandler<Query, long>()).Return(query);

            controller = new SystemController(queryFactory);
            SetupControllerContext(controller);
        }

        [Test]
        public void Should_behave_as_expected()
        {
            const long migration = 5;
            string version = GetType().Assembly.GetName().Version.ToString();
            query.Expect(q => q.Execute(Arg<Query>.Is.Anything)).Return(migration);

            var result = controller.Version();

            Assert.IsInstanceOf<VersionModel>(result.Model);

            var model = (VersionModel) result.Model;
            Assert.AreEqual(version, model.Assembly);
            Assert.AreEqual(migration.ToString(), model.Schema);
            query.VerifyAllExpectations();
        }
    }
}