using System;
using NUnit.Framework;

namespace Nebula.IntegrationTests
{
    [TestFixture]
    public class MappingTests : Fixture
    {
        [Test]
        [Category("Smoke")]
        public void Should_be_able_to_query_all_mapped_classes()
        {
            using (var session = SessionFactory.OpenSession())
            {
                foreach (var persistentClass in Configuration.ClassMappings)
                {
                    try
                    {
                        var query = session
                            .CreateQuery(string.Format("from {0}", persistentClass.MappedClass.Name))
                            .SetMaxResults(1);

                        query.List();
                    }
                    catch (Exception e)
                    {
                        Assert.Fail("Mapping issue encountered with {0}\r\n{1}", persistentClass.MappedClass.Name, e);
                    }
                }
            }
        }
    }
}