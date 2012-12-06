using Castle.DynamicProxy;

namespace Nebula.Infrastructure
{
    public static class IInvocationExtensions
    {
        public static string TypeQualifiedMethodName(this IInvocation invocation)
        {
            return string.Format("{0}.{1}", invocation.TargetType == null ? "" : invocation.TargetType.Name,
                                 invocation.Method.Name);
        }
    }
}