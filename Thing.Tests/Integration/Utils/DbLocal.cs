using System;
using System.Data.Entity;
using System.IO;
using System.Threading;

namespace Thing.Tests.Integration.Utils
{
    public class DbLocal : IDisposable
    {
        public static Func<string, DbContext> DbContextFactory = null;
        private static readonly ThreadLocal<DbContext> Instance = new ThreadLocal<DbContext>();

        private DbLocal(string dbName)
        {
            if (DbContextFactory == null)
            {
                throw new InvalidOperationException("Cannot create DbContextFactory");
            }

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppData");

            if (AppDomain.CurrentDomain.GetData("DataDirectory") == null)
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string connectionString =
                $@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Db-{dbName}.mdf;Integrated Security=True;MultipleActiveResultSets=True";
            Instance.Value = DbContextFactory(connectionString);
            Instance.Value.Database.CreateIfNotExists();
        }

        public void Dispose()
        {
            Instance.Value.Database.Connection.Close();
            Instance.Value.Database.Delete();
        }

        public static DbLocal Create(Type context)
        {
            return new DbLocal(context.Name);
        }

        public static string GetConnectionString() => Instance.Value.Database.Connection.ConnectionString;

        public static T GetTypedDbContext<T>() where T : DbContext
        {
            return Instance.Value as T;
        }        
    }
}