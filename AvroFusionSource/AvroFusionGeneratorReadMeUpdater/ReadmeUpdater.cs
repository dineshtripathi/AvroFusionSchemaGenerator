public class ReadmeUpdater : IReadmeUpdater
{
    public void UpdateReadmeFile(string packageName, string packageVersion, string tag)
    {
        var lines = File.ReadAllLines("README.md");
        var updatedLines = InsertRowInReadmeTable(lines, packageName, packageVersion, tag);
        File.WriteAllLines("README.md", updatedLines);
    }

    private int FindHeaderLine(string[] lines)
    {
        return Array.FindIndex(lines, line => line.Contains("| Package Name    | Package Version | Tag |"));
    }
   
    private string[] InsertRowInReadmeTable(string[] lines, string packageName, string packageVersion, string tag)
    {
        int headerLine = FindHeaderLine(lines);

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