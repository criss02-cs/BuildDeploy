using System.ComponentModel;

namespace BuildDeploy.Business.Models;
public record FileInfo(string Path, string Name, string Size, DateTime LastModified, bool IsSelected, Tipo Tipo);

public enum Tipo
{
    File,
    [Description("Cartella")]
    Folder
}
