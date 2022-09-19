namespace MedBot; 

/// <summary>
/// Class used to load environment variables from a file.
/// </summary>
public static class DotEnv
{
    /// <summary>
    /// Loads environment variables from the specified file into the programs process.
    /// </summary>
    /// <param name="filePath">The path to the .env file.</param>
    public static void LoadEnvironmentVariables(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("ERROR: No .env file found.");
            return;
        }

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}