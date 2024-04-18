using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildDeploy.Utils
{
    public class ServiceManager
    {
        public static T? GetService<T>() where T : class
        {
            var service = Application.Current?.Handler.MauiContext?.Services.GetService<T>();
            return service;
        }

        public static object? GetService(Type type)
        {
            var service = Application.Current?.Handler.MauiContext?.Services.GetService(type);
            return service;
        }
    }
}
