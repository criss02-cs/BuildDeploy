using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildDeploy.Business.Models;

public class HierarchyItem(string description, params HierarchyItem[] items)
{
    public string? Description { get; set; } = description;
    public ObservableCollection<HierarchyItem> Items { get; set; } = new(items);

    public static HierarchyItem BuildHierarchyFromPath(string path)
    {
        var parts = path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
        return BuildHierarchyRecursive(parts, 0);
    }

    private static HierarchyItem BuildHierarchyRecursive(string[] parts, int index)
    {
        if (index >= parts.Length)
        {
            return null;
        }

        var currentPart = parts[index];
        var child = BuildHierarchyRecursive(parts, index + 1);

        if (child == null)
        {
            return new HierarchyItem(currentPart);
        }
        else
        {
            return new HierarchyItem(currentPart, child);
        }
    }
}
