using System.ComponentModel;

namespace BuildDeploy.Business.Models;

public record FileInfo(string Path, string Name, string Size, DateTime LastModified, Tipo Tipo)
{
    public bool IsSelected { get; set; }
}

public enum Tipo
{
    File,
    [Description("Cartella")]
    Folder
}
