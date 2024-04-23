using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildDeployWpf.Extensions;
using BuildDeployWpf.Messages;
using BuildDeployWpf.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace BuildDeployWpf.ViewModels;

public partial class FolderListViewModel : BaseViewModel, IRecipient<Appearing>
{
    [ObservableProperty] private ObservableCollection<Folder> _folders = [];

    public FolderListViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }
    
    public void Receive(Appearing message)
    {
        List<Folder> folders = [BuildTreeView(message.Value)];
        Folders.AddRange(folders);
    }

    private Folder BuildTreeView(string path)
    {
        var d = new DirectoryInfo(path);
        var folder = new Folder
        {
            Path = path,
            Name = d.Name
        };
        foreach (var dir in d.GetDirectories())
        {
            folder.SubFolders.Add(BuildTreeView(dir.FullName));
        }

        return folder;
    }
}