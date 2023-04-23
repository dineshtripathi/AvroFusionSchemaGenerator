using NuGet.Versioning;

public class ReadmeUpdater : IReadmeUpdater
{
    
    public void UpdateReadmeFile(string packageName, string packageVersion, string tag)
    {
        var workspacePath = GetEnvironmentVariableWithMessage("GITHUB_WORKSPACE", "GITHUB_WORKSPACE");
        LogMessage(workspacePath);
        var readmeFilePath = Path.Combine(workspacePath, "README.md");
        LogMessage(readmeFilePath);
        var lines = File.ReadAllLines(readmeFilePath);
        var updatedLines = InsertRowInReadmeTable(lines, packageName, packageVersion, tag);
        LogMessage(updatedLines.FirstOrDefault());
        LogMessage(packageName);
        LogMessage(tag);
        File.WriteAllLines("README.md", updatedLines);
    }

    private int FindHeaderLine(string[] lines)
    {
        return Array.FindIndex(lines, line => line.Contains("| Package Name    | Package Version | Tag |"));
    }

    private string[] InsertRowInReadmeTable(string[] lines, string packageName, string packageVersion, string tag)
    {
        var headerLine = FindHeaderLine(lines);
      
        if (headerLine == -1)
        {
            return lines;
        }

        var newRow = $"| {packageName} | {packageVersion} | {tag} |";
        LogMessage(newRow);
        return InsertRowInLines(lines, headerLine + 2, newRow);
    }

    private string[] InsertRowInLines(string[] lines, int index, string newRow)
    {
        var newLines = new string[lines.Length + 1];
        Array.Copy(lines, 0, newLines, 0, index);
        newLines[index] = newRow;
        Array.Copy(lines, index, newLines, index + 1, lines.Length - index);
        return newLines;
    }

    private string GetEnvironmentVariableWithMessage(string variable, string message)
    {
        var value = Environment.GetEnvironmentVariable(variable);
        Console.WriteLine($"{message} : {value}");
        return value;
    }

    private void LogMessage(string message)
    {
        Console.WriteLine("-----------------------------");
        Console.WriteLine(message);
        Console.WriteLine("-----------------------------");
    }
}