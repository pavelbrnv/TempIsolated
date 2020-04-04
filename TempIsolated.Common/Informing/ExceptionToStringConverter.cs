using System;
using System.Text;

namespace TempIsolated.Common.Informing
{
	public static class ExceptionToStringConverter
	{
		public static string Convert(Exception ex)
		{
			var result = new StringBuilder();
			Append(result, ex, 0);
			return result.ToString();
		}

		public static void AppendNextLine(StringBuilder result, Exception ex, int indentLevel)
		{
			var indent = GetNewLine(indentLevel);
			result.Append(indent);

			Append(result, ex, indentLevel);
		}

		public static void Append(StringBuilder result, Exception ex, int indentLevel)
		{
			var newLine = GetNewLine(indentLevel);

			var message = ex.Message.Replace(Environment.NewLine, newLine);
			var stackTrace = ex.StackTrace?.Replace(Environment.NewLine, newLine);

			var tmp = new StringBuilder($"{message}{newLine}{ex.GetType()}{newLine}{stackTrace ?? "<StackTrace=null>"}");

			result.Append(tmp);

			if (ex.InnerException != null)
			{
				result.Append($"{newLine}inner exception:");
				AppendNextLine(result, ex.InnerException, indentLevel + 1);
			}
		}

		#region Private

		private static string GetNewLine(int indentLevel)
		{
			var indent = new string('	', indentLevel);
			return Environment.NewLine + indent;
		}

		#endregion
	}
}
