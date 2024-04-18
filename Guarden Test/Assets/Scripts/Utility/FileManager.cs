using System.IO;

namespace TheGuarden.Utility
{
    public static class FileManager
    {
        public static string ReadFile(string path, bool throwOnError = false)
        {
            if (!File.Exists(path))
            {
                GameLogger.LogError($"File {path} does not exist", null, GameLogger.LogCategory.FileOperations);
                return throwOnError ? throw new FileNotFoundException(path) : "";
            }

            return File.ReadAllText(path);
        }

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
