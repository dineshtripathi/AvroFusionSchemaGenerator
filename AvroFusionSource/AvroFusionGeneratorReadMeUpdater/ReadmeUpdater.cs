using NuGet.Versioning;

public class ReadmeUpdater : IReadmeUpdater
{
    //public void GetPackage()
    //{
    //    string workspacePath = GetEnvironmentVariableWithMessage("GITHUB_WORKSPACE", "GITHUB_WORKSPACE");
    //    string nugetPackagePath = GetEnvironmentVariableWithMessage("NUGET_PACKAGE_PATH", "NUGET_PACKAGE_PATH");
    //    string workSpacewithPackagePath = Path.Combine(workspacePath, nugetPackagePath);

    //    LogMessage($"WorkSpaceWithPackagePath :- {workSpacewithPackagePath}");

    //    var latestNuGetPackage = Directory.GetFiles(workSpacewithPackagePath, "*.nupkg")
    //        .MaxBy(f => new FileInfo(f).CreationTime);

    //    LogMessage($"latestNuGetPackage :- {latestNuGetPackage}");

    //    if (!string.IsNullOrEmpty(latestNuGetPackage))
    //    {
    //        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(latestNuGetPackage);
    //        var nameAndVersion = fileNameWithoutExtension.Split('.');

    //        var packageName = string.Join(".", nameAndVersion.Take(nameAndVersion.Length - 1));
    //        var packageVersion = string.Join(".", nameAndVersion.Skip(nameAndVersion.Length - 1));

    //        var tag = "v" + packageVersion;

    //        UpdateReadmeFile(packageName, packageVersion, tag);
    //    }
    //    else
    //    {
    //        Console.WriteLine("No NuGet package found in the specified directory.");
    //    }
    //}

    public void UpdateReadmeFile(string packageName, string packageVersion, string tag)
    {
        var workspacePath = GetEnvironmentVariableWithMessage("GITHUB_WORKSPACE", "GITHUB_WORKSPACE");
        var readmeFilePath = Path.Combine(workspacePath, "README.md");

        var lines = File.ReadAllLines(readmeFilePath);
        var updatedLines = InsertRowInReadmeTable(lines, packageName, packageVersion, tag);
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