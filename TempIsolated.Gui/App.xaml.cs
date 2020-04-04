using System;
using System.Windows;
using TempIsolated.Common.Gui.Informing;
using TempIsolated.Common.Informing;

namespace TempIsolated.Gui
{
    public partial class App : Application
    {
        private readonly ILogger logger = new InfoWindowLogger();

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleDomainUnhandledException;
            Dispatcher.UnhandledException += HandleDispatcherUnhandledException;
        }

        private void HandleDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.LogError(Gui.Properties.Resources.UnhandledErrorMessage, (Exception)e.ExceptionObject);
        }

        private void HandleDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger.LogError(Gui.Properties.Resources.UnhandledErrorMessage, e.Exception);
        }
    }
}
