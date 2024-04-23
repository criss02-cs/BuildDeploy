namespace BuildDeployWpf.Models;
public class Folder
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public List<Folder> SubFolders { get; set; } = [];
}