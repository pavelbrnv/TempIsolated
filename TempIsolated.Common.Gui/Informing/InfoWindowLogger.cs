using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;

namespace TempIsolated.Common.Gui.Informing
{
    public sealed class InfoWindowLogger : ILogger
    {
        public void LogMessage(LogMessageType type, string message)
        {
            InfoWindow.ShowDialog(Properties.Resources.ResourceManager.TryLocalize(type), message);
        }

        public void LogMessage(LogMessageType type, string message, Exception e)
        {
            InfoWindow.ShowDialog(Properties.Resources.ResourceManager.TryLocalize(type), message, ExceptionToStringConverter.Convert(e));
        }
    }
}
