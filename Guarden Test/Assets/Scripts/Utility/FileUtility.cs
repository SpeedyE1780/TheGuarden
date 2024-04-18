using System.IO;

namespace TheGuarden.Utility
{
    /// <summary>
    /// FileUtility is a wrapper class around common IO functionality
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// Wrapper function around File.ReadAllText that does the necessary error handling
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="throwOnError">Throws error if true</param>
        /// <returns>The file content if it exists else an empty string</returns>
        /// <exception cref="FileNotFoundException">If throwOnError is true throws when file does not exist</exception>
        public static string ReadFile(string path, bool throwOnError = false)
        {
            if (!File.Exists(path))
            {
                GameLogger.LogError($"File {path} does not exist", null, GameLogger.LogCategory.FileOperations);
                return throwOnError ? throw new FileNotFoundException(path) : "";
            }

            return File.ReadAllText(path);
        }

        /// <summary>
        /// Wrapper function around File.WriteAllText that does the necessary error handling
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="content">Content written to file</param>
        /// <param name="throwOnError">Throws error if true</param>
        /// <exception cref="DirectoryNotFoundException">If throwOnError is true throws when directory does not exist</exception>
        public static void WriteFile(string path, string content, bool throwOnError = false)
        {
            string directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
            {
                GameLogger.LogError($"Directory {directory} does not exist", null, GameLogger.LogCategory.FileOperations);

                if (throwOnError)
                {
                    throw new DirectoryNotFoundException(path);
                }

                GameLogger.LogInfo($"Directory {directory} created", null, GameLogger.LogCategory.FileOperations);
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, content);
        }
    }
}
