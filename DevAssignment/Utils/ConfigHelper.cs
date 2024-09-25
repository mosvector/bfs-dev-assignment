using System;
using System.Configuration;

namespace DevAssignment.Utils
{
    /// <summary>
    /// Configuration helper
    /// </summary>
    internal static class ConfigHelper
    {
        /// <summary>
        /// Get the app setting value from environment variables and the configuration.
        /// </summary>
        /// <param name="key">Config key</param>
        /// <returns>Value of config</returns>
        public static string GetConfigValue(string key)
        {
            // Get the value from environment variables first, if not found then get it from the configuration file
            return Environment.GetEnvironmentVariable(key.ToUpper()) ?? ConfigurationManager.AppSettings[key];
        }
    }
}
