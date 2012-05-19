using System.Linq;
using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Nebula.Infrastructure.Commanding;

namespace Nebula.Infrastructure
{
    public static class WindsorContainerExtensions
    {
        public static void AddFacilityIfNeeded<T>(this IWindsorContainer container) where T : IFacility, new()
        {
            if (!container.Kernel.GetFacilities().Any(f => f.GetType() == typeof (T)))
                container.AddFacility<T>();
        }

        public static void RegisterCommandHandlers(this IWindsorContainer container, Assembly assembly)
        {
            container.Register(AllTypes.FromAssembly(assembly)
                                   .BasedOn(typeof (ICommandHandler<>))
                                   .WithServiceBase()
                                   .LifestyleTransient());
        }
    }
}