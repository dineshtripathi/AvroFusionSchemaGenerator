public class ReadmeUpdater : IReadmeUpdater
{
    
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

    private static int FindHeaderLine(string?[] lines)
    {
        return Array.FindIndex(lines, line => line.Contains("| Download Package| Package Name    | Package Version | Tag |"));
    }

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

    private static string?[] InsertRowInLines(string?[] lines, int index, string? newRow)
    {
        var newLines = new string?[lines.Length + 1];
        Array.Copy(lines, 0, newLines, 0, index);
        newLines[index] = newRow;
        Array.Copy(lines, index, newLines, index + 1, lines.Length - index);
        return newLines;
    }

    private static string? GetEnvironmentVariableWithMessage(string variable, string message)
    {
        var value = Environment.GetEnvironmentVariable(variable);
        Console.WriteLine($"{message} : {value}");
        return value;
    }

    private static void LogMessage(string? message)
    {
        Console.WriteLine("-----------------------------");
        Console.WriteLine(message);
        Console.WriteLine("-----------------------------");
    }
}