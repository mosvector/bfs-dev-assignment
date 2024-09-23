using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace DevAssignment.Tests
{
    /// <summary>
    /// Test main program for the DevAssignment project
    /// </summary>
    [TestClass]
    public class ProgramTests
    {
        /// <summary>
        /// Test the main program with valid arguments
        /// </summary>
        [TestMethod]
        public void TestMainValidArguments()
        {
            // Arrange
            string inputFilePath = "input_valid.txt";
            string outputFilePath = "output_valid.txt";

            // Create a sample input file
            File.WriteAllText(inputFilePath, "Hello world! This is a test file.");

            // Use reflection to get the private static method
            MethodInfo methodInfo = typeof(Program).GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(methodInfo, "Main method not found");

            // Act
            int result = (int)methodInfo.Invoke(null, new object[] { new string[] { inputFilePath, outputFilePath } });

            // Assert
            Assert.IsTrue(File.Exists(inputFilePath));
            Assert.IsTrue(File.Exists(outputFilePath));
            Assert.AreEqual(0, result);

            // Clean up
            File.Delete(inputFilePath);
            File.Delete(outputFilePath);
        }

        /// <summary>
        /// Test the main program with missing arguments
        /// </summary>
        [TestMethod]
        public void TestMainMissingArguments()
        {
            // Arrange
            string inputFilePath = "input_missing.txt";
            string outputFilePath = "output_missing.txt";

            // Create a sample input file
            File.WriteAllText(inputFilePath, "Hello world! This is a test file.");

            // Use reflection to get the private static method
            MethodInfo methodInfo = typeof(Program).GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(methodInfo, "Main method not found");

            // Act
            int result = (int) methodInfo.Invoke(null, new object[] { new string[] { inputFilePath } });

            // Assert
            Assert.IsFalse(File.Exists(outputFilePath));
            Assert.AreEqual(-2, result);

            // Clean up
            if (File.Exists(inputFilePath))
            {
                File.Delete(inputFilePath);
            }

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
        }

        /// <summary>
        /// Test the main program with input file not exists
        /// </summary>
        [TestMethod]
        public void TestMainFileNotExists()
        {
            // Arrange
            string inputFilePath = "input_notfound.txt";
            string outputFilePath = "output_notfound.txt";

            // No file created

            // Use reflection to get the private static method
            MethodInfo methodInfo = typeof(Program).GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(methodInfo, "Main method not found");

            // Act
            int result = (int)methodInfo.Invoke(null, new object[] { new string[] { inputFilePath, outputFilePath } });

            // Assert
            Assert.IsFalse(File.Exists(outputFilePath));
            Assert.AreEqual(-1, result);

            // Clean up
            if (File.Exists(inputFilePath))
            {
                File.Delete(inputFilePath);
            }

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
        }
    }
}
