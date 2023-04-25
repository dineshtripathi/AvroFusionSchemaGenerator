public class ReadmeUpdater : IReadmeUpdater
{

    /// <summary>
    /// Updates the readme file.
    /// </summary>
    /// <param name="packageUrl">The package url.</param>
    /// <param name="packageName">The package name.</param>
    /// <param name="packageVersion">The package version.</param>
    /// <param name="tag">The tag.</param>
    public void UpdateReadmeFile(string packageUrl, string packageName, string packageVersion, string tag)
    {
        var workspacePath = GetEnvironmentVariableWithMessage("GITHUB_WORKSPACE", "GITHUB_WORKSPACE");
        LogMessage(workspacePath);
        var readmeFilePath = Path.Combine(workspacePath, "README.md");
        LogMessage(readmeFilePath);
        string?[] lines = File.ReadAllLines(readmeFilePath);
        var updatedLines = InsertRowInReadmeTable(lines, packageUrl,packageName, packageVersion, tag);
        File.WriteAllLines("README.md", updatedLines);
    }

    /// <summary>
    /// Finds the header line.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <returns>An int.</returns>
    private static int FindHeaderLine(string?[] lines)
    {
        return Array.FindIndex(lines, line => line.Contains("| Download Package| Package Name    | Package Version | Tag |"));
    }

    /// <summary>
    /// Inserts the row in readme table.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <param name="packageUrl">The package url.</param>
    /// <param name="packageName">The package name.</param>
    /// <param name="packageVersion">The package version.</param>
    /// <param name="tag">The tag.</param>
    /// <returns>An array of string?.</returns>
    private static string?[] InsertRowInReadmeTable(string?[] lines, string packageUrl,string packageName, string packageVersion, string tag)
    {
        var headerLine = FindHeaderLine(lines);
      
        if (headerLine == -1)
        {
            return lines;
        }
        var newRow = $"|[Avro Fusion Generator]({new Uri(packageUrl)})| {packageName} | {packageVersion} | {tag} |";
        LogMessage(newRow);
        return InsertRowInLines(lines, headerLine + 2, newRow);
    }

    /// <summary>
    /// Inserts the row in lines.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <param name="index">The index.</param>
    /// <param name="newRow">The new row.</param>
    /// <returns>An array of string?.</returns>
    private static string?[] InsertRowInLines(string?[] lines, int index, string? newRow)
    {
        var newLines = new string?[lines.Length + 1];
        Array.Copy(lines, 0, newLines, 0, index);
        newLines[index] = newRow;
        Array.Copy(lines, index, newLines, index + 1, lines.Length - index);
        return newLines;
    }

    /// <summary>
    /// Gets the environment variable with message.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <param name="message">The message.</param>
    /// <returns>A string? .</returns>
    private static string? GetEnvironmentVariableWithMessage(string variable, string message)
    {
        var value = Environment.GetEnvironmentVariable(variable);
        Console.WriteLine($"{message} : {value}");
        return value;
    }

    /// <summary>
    /// Logs the message.
    /// </summary>
    /// <param name="message">The message.</param>
    private static void LogMessage(string? message)
    {
        Console.WriteLine("-----------------------------");
        Console.WriteLine(message);
        Console.WriteLine("-----------------------------");
    }
}