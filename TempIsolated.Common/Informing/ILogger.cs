using System;

namespace TempIsolated.Common.Informing
{
	public interface ILogger
	{
		void LogMessage(LogMessageType type, string message);

		void LogMessage(LogMessageType type, string message, Exception e);
	}

	public enum LogMessageType
	{
		Debug,
		Information,
		Warning,
		Error
	}
}
