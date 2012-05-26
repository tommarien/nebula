﻿using System.Linq;
using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;

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
            container.Register(Classes.FromAssembly(assembly)
                                   .BasedOn<ICommandHandler>()
                                   .WithServiceFirstInterface()
                                   .LifestyleTransient());
        }

        public static void RegisterQueries(this IWindsorContainer container, Assembly assembly)
        {
            container.Register(Classes.FromAssembly(assembly)
                                   .BasedOn(typeof (IQuery<,>))
                                   .WithServiceBase()
                                   .LifestyleTransient());
        }
    }
}