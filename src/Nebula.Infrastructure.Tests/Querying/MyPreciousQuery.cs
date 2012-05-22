using Nebula.Infrastructure.Querying;

namespace Nebula.Infrastructure.Tests.Querying
{
    public class MyPrecious
    {
        public int Result { get; set; }
    }

    public class MyPreciousQueryById : IQuery<int, MyPrecious>
    {
        public MyPrecious Execute(int input)
        {
            return new MyPrecious {Result = input};
        }
    }
}