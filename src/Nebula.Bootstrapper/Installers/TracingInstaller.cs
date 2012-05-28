﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Nebula.Bootstrapper.Installers
{
    public class TracingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<TracingInterceptor>().LifestylePerWebRequest());
        }
    }
}