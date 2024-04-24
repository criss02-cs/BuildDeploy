using BuildDeploy.Business.Models;

namespace BuildDeploy.Business.Utils;

public class Utils
{
    public static string FormatBytes(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        var order = 0;
        while (bytes >= 1024 && order < sizes.Length - 1)
        {
            order++;
            bytes /= 1024;
        }

        return $"{bytes:0.##} {sizes[order]}";
    }

    public static Folder FindFolderAndSubFolders(string path)
    {
        var d = new DirectoryInfo(path);
        var folder = new Folder
        {
            Path = path,
            Name = d.Name
        };
        foreach (var dir in d.GetDirectories())
        {
            folder.SubFolders.Add(FindFolderAndSubFolders(dir.FullName));
        }

        return folder;
    }
}