using System;
using System.Threading.Tasks;

namespace TempIsolated.Common.Informing
{
	public static class LoggerExtensions
	{
		#region Public

		public static void LogInfo(this ILogger logger, string message)
		{
			logger.LogMessage(LogMessageType.Information, message);
		}

		public static void LogInfo(this ILogger logger, string format, params object[] args)
		{
			logger.LogMessage(LogMessageType.Information, format, args);
		}

		public static void LogWarning(this ILogger logger, string message)
		{
			logger.LogMessage(LogMessageType.Warning, message);
		}

		public static void LogWarning(this ILogger logger, string format, params object[] args)
		{
			logger.LogMessage(LogMessageType.Warning, format, args);
		}

		public static void LogWarning(this ILogger logger, string message, Exception e)
		{
			logger.LogMessage(LogMessageType.Warning, message, e);
		}

		public static void LogError(this ILogger logger, string message)
		{
			logger.LogMessage(LogMessageType.Error, message);
		}

		public static void LogError(this ILogger logger, string format, params object[] args)
		{
			logger.LogMessage(LogMessageType.Error, format, args);
		}

		public static void LogError(this ILogger logger, string message, Exception e)
		{
			logger.LogMessage(LogMessageType.Error, message, e);
		}

		public static void LogDebug(this ILogger logger, string message)
		{
			logger.LogMessage(LogMessageType.Debug, message);
		}

		public static void LogDebug(this ILogger logger, string format, params object[] args)
		{
			logger.LogMessage(LogMessageType.Debug, format, args);
		}

		public static async void ForgetSafely(this Task task, ILogger logger, string messageOnError)
		{
			try
			{
				await task;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception e)
			{
				logger.LogError(messageOnError, e);
			}
		}

		public static async Task Safely(this Task task, ILogger logger, string messageOnError)
		{
			try
			{
				await task;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception e)
			{
				logger.LogError(messageOnError, e);
			}
		}

		#endregion

		#region Private

		private static void LogMessage(this ILogger logger, LogMessageType type, string format, params object[] args)
		{
			logger.LogMessage(type, string.Format(format, args));
		}

		#endregion
	}
}
