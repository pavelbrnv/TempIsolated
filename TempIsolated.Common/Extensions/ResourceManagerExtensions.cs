using System.Resources;

namespace TempIsolated.Common.Extensions
{
	public static class ResourceManagerExtensions
	{
		public static string TryLocalize(this ResourceManager resourceManager, object value)
		{
			if (value == null)
			{
				return string.Empty;
			}

			var stringValue = value.ToString();

			if (resourceManager == null)
			{
				return stringValue;
			}

			var localizedValue = resourceManager.GetString(stringValue);
			if (localizedValue != null)
			{
				return localizedValue;
			}

			return stringValue;
		}
	}
}
