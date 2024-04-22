using System.IO;
using SQLite;

namespace BuildDeployWpf.Database
{
    internal static class DbConstants
    {
        public const string DatabaseFilename = "builddeploy.db3";
        public const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;

        public static string DatabasePath => Path.Combine("", DatabaseFilename);
    }
}
