using NuGet.Versioning;
public class ReadmeUpdater : IReadmeUpdater
{
    public void GetPackage()
    {

        const string packageDirectory = "AvroFusionSource/AvroFusionGenerator/nupkg/";
        var latestNuGetPackage = Directory.GetFiles(packageDirectory, "*.nupkg").MaxBy(f => new FileInfo(f).CreationTime);

        if (!string.IsNullOrEmpty(latestNuGetPackage))
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(latestNuGetPackage);
            var nameAndVersion = fileNameWithoutExtension.Split('.');

            var packageName = string.Join(".", nameAndVersion.Take(nameAndVersion.Length - 1));
            var packageVersion = string.Join(".", nameAndVersion.Skip(nameAndVersion.Length - 1));

            var tag = "v" + packageVersion;

            UpdateReadmeFile(packageName, packageVersion, tag);
        }
        else
        {
            Console.WriteLine("No NuGet package found in the specified directory.");
        }

    }
    public void UpdateReadmeFile(string packageName, string packageVersion, string tag)
    {
        var workspacePath = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");
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

}