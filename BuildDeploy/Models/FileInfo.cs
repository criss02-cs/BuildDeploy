using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildDeploy.Models;
public record FileInfo(string Path, string Name, string Size, DateTime LastModified, bool IsSelected, Tipo Tipo);

public enum Tipo
{
    File,
    [Description("Cartella")]
    Folder
}
