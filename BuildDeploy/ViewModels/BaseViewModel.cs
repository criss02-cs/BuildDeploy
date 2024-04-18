using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildDeploy.Database;
using BuildDeploy.Utils;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BuildDeploy.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    protected readonly DbService? DbService = ServiceManager.GetService<DbService>();
}
