using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildDeploy.Models;
public class Folder
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public List<Folder> SubFolders { get; set; } = [];
}