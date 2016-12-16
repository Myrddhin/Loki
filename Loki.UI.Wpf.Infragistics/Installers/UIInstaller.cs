﻿using Loki.IoC;

namespace Loki.UI.Wpf.Infragistics
{
    public class UIInstaller : LokiContextInstaller
    {
        public override void Install(IObjectContext context)
        {
            // context.Register(Element.For<IThreadingContext>().ImplementedBy<WpfThreadingContext>());
            // context.Register(Element.For<ITemplatingEngine>().ImplementedBy<InfragisticsWpfTemplatingEngine>());
            // context.Register(Element.For<ISignalManager>().ImplementedBy<WpfSignalManager>());
            // context.Register(Element.For<IWindowManager>().ImplementedBy<WpfWindowManager>());
            // context.Register(Element.For<DefaultSplashModel>());
        }

        public static UIInstaller Wpf { get; } = new UIInstaller();
    }
}