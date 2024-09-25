using DevAssignment.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace DevAssignment.Tests.Utils
{
    /// <summary>
    /// Test class for the ConfigHelper class
    /// </summary>
    [TestClass]
    public class ConfigHelperTests
    {
        /// <summary>
        /// Test the value from environment variable
        /// </summary>
        [TestMethod]
        public void TestGetConfigValueFromEnvironment()
        {
            // Arrange
            var key = "TestKey";
            var value = "TestValue";
            Environment.SetEnvironmentVariable(key, value);

            // Act
            var result = ConfigHelper.GetConfigValue(key);

            // Assert
            Assert.AreEqual(value, result);
        }

        /// <summary>
        /// Test the value from app.config
        /// </summary>
        [TestMethod]
        public void TestGetConfigValueFromAppConfig()
        {
            // Arrange
            var key = "TestKey";
            var value = "TestValue";
            ConfigurationManager.AppSettings[key] = value;

            // Act
            var result = ConfigHelper.GetConfigValue(key);

            // Assert
            Assert.AreEqual(value, result);
        }

        /// <summary>
        /// Test the value from environment variable takes precedence over the value from app.config
        /// </summary>
        [TestMethod]
        public void TestGetConfigValueFromEnvironmentAndAppConfig()
        {
            // Arrange
            var key = "TestKey";
            var valueFromEnvVar = "TestValue";
            var valueFromAppConfig = "TestValue2";
            Environment.SetEnvironmentVariable(key, valueFromEnvVar);
            ConfigurationManager.AppSettings[key] = valueFromAppConfig;

            // Act
            var result = ConfigHelper.GetConfigValue(key);

            // Assert
            Assert.AreEqual(valueFromEnvVar, result);
        }
    }
}
