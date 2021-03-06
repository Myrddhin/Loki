﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Loki.UI.Commands;

namespace Loki.UI.Wpf.Infragistics.Installers
{
    public class UIInfrastructureInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDisplayInfrastructure>().ImplementedBy<DisplayInfrastructure>());
            container.Register(Component.For<ICommandManager>().ImplementedBy<CommandManager>());
            container.Register(Component.For<ITemplateManager>().ImplementedBy<TemplateManager>());
        }
    }
}