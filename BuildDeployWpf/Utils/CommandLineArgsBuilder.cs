using BuildDeployWpf.Models;

namespace BuildDeployWpf.Utils;

public class CommandLineArgsBuilder
{
    public static CommandLineArgs Build()
    {
        var argsDic = Environment.GetCommandLineArgs()
            .FirstOrDefault(x => !x.Contains("BuildDeployWpf.dll"))
            ?.Split(';')
            .ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]) ?? [];
        var args = new CommandLineArgs();
        args.SplashScreen = argsDic.TryGetValue(nameof(args.SplashScreen), out var splash) ? Convert.ToBoolean(splash) : null;
        args.ProjectPath = argsDic.GetValueOrDefault(nameof(args.ProjectPath), "");
        return args;
    }
}