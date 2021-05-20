using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;

namespace PluginManager.Data.Models
{
    public partial class PmDb
    {
        private const string DBNAME = "pmdb.sqlite";
        private const string DBPATH = "";

        private static string connectionString = "Data Source=\"D:\\Projects\\WPF Projects\\PluginManager\\pmdb.sqlite\"";

        public static void BuildConnectString(string dbPath = DBPATH, string dbName = DBNAME)
        {
            Debug.Assert(dbName != null);
            Debug.Assert(dbPath != null);

            var path = Path.GetFullPath(Path.Combine(dbPath, dbName));
            connectionString = $"Data Source=\"{path}\"";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(connectionString);
            }
        }
    }
}