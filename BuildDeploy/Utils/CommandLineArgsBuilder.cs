using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildDeploy.Models;

namespace BuildDeploy.Utils;

public class CommandLineArgsBuilder
{
    public static CommandLineArgs Build()
    {
        var argsDic = Environment.GetCommandLineArgs()
            .FirstOrDefault(x => !x.Contains("BuildDeploy.dll"))?.Split(';')
            .ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]) ?? [];
        var args = new CommandLineArgs();
        args.SplashScreen = argsDic.TryGetValue(nameof(args.SplashScreen), out var splash) ? Convert.ToBoolean(splash) : null;
        args.ProjectPath = argsDic.GetValueOrDefault(nameof(args.ProjectPath), "");
        return args;
    }
}