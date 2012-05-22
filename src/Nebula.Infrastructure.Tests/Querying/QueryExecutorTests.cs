using System;
using NUnit.Framework;
using Nebula.Infrastructure.Querying;
using Rhino.Mocks;

namespace Nebula.Infrastructure.Tests.Querying
{
    [TestFixture]
    public class QueryExecutorTests
    {
        [SetUp]
        public void Setup()
        {
            queryFactory = MockRepository.GenerateMock<IQueryFactory>();
            queryExecutor = new QueryExecutor(queryFactory);
        }

        private IQueryFactory queryFactory;
        private IQueryExecutor queryExecutor;

        [Test]
        public void Execute_Should_execute_the_query()
        {
            var query = MockRepository.GenerateStrictMock<IQuery<int, MyPrecious>>();
            queryFactory.Stub(f => f.CreateQuery<int, MyPrecious>()).Return(query);
            var precious = new MyPrecious();
            query.Expect(q => q.Execute(5)).Return(precious);

            Assert.AreSame(precious, queryExecutor.Execute<int, MyPrecious>(5));

            query.VerifyAllExpectations();
        }

        [Test]
        public void Execute_Should_get_an_instance_of_the_query_from_the_factory()
        {
            queryFactory.Expect(f => f.CreateQuery<int, MyPrecious>()).Return(new MyPreciousQueryById());

            queryExecutor.Execute<int, MyPrecious>(1);

            queryFactory.VerifyAllExpectations();
        }

        [Test]
        public void Execute_Should_release_the_query()
        {
            var query = MockRepository.GenerateMock<IQuery<int, MyPrecious>>();
            queryFactory.Stub(f => f.CreateQuery<int, MyPrecious>()).Return(query);
            queryFactory.Expect(f => f.Release(query));
            var precious = new MyPrecious();
            query.Stub(q => q.Execute(5)).Return(precious);

            queryExecutor.Execute<int, MyPrecious>(5);

            queryFactory.VerifyAllExpectations();
        }

        [Test]
        public void Execute_Should_release_the_query_no_matter_what()
        {
            var query = MockRepository.GenerateMock<IQuery<int, MyPrecious>>();
            queryFactory.Stub(f => f.CreateQuery<int, MyPrecious>()).Return(query);
            queryFactory.Expect(f => f.Release(query));
            query.Stub(q => q.Execute(5)).Throw(new ArgumentException());

            try
            {
                queryExecutor.Execute<int, MyPrecious>(5);
            }
            catch (Exception)
            {
            }

            queryFactory.VerifyAllExpectations();
        }
    }
}