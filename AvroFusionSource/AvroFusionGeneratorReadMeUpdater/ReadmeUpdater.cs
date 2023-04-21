public class ReadmeUpdater : IReadmeUpdater
{
    public void UpdateReadmeFile(string packageVersion, string tag)
    {
        var lines = File.ReadAllLines("README.md");

        // Find the line number of the table header
        int headerLine = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("| Package Version | Tag |"))
            {
                headerLine = i;
                break;
            }
        }

        if (headerLine != -1)
        {
            // Insert a new row to the table with package version and tag
            var newRow = $"| {packageVersion} | {tag} |";
            var newLines = new string[lines.Length + 1];
            Array.Copy(lines, 0, newLines, 0, headerLine + 2);
            newLines[headerLine + 2] = newRow;
            Array.Copy(lines, headerLine + 2, newLines, headerLine + 3, lines.Length - headerLine - 2);
            File.WriteAllLines("README.md", newLines);
        }
    }
}