﻿namespace Edi.Themes
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Edi.Themes.Interfaces;

    /// <summary>
    /// This class gets picked up by from Castle.Windsor because
    /// it implements the <see cref="IWindsorInstaller"/> interface.
    /// 
    /// The <see cref="IWindsorInstaller"/> interface is used by the
    /// container to resolve installers when calling
    /// <see cref="IWindsorContainer"/>.Install(FromAssembly.This()); 
    /// </summary>
    public class Installers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container,
                            IConfigurationStore store)
        {
            // Register Application Helper class to help castle satisfy dependencies on it
            container
                .Register(Component.For<IThemesManager>()
                .ImplementedBy<ThemesManager>().LifestyleSingleton());
        }
    }
}