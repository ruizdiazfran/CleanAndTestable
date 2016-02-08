using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace Thing.Tests.Integration.Utils
{
    public sealed class DbLocal : IDisposable
    {
        public static Func<string, DbContext> DbContextFactory = null;
        private static readonly ThreadLocal<DbContext> Instance = new ThreadLocal<DbContext>();

        private readonly SqlConnectionStringBuilder _connectionStringBuilder =
            new SqlConnectionStringBuilder(
                @"Data Source=(LocalDb)\MSSQLLocalDB;Integrated Security=True;MultipleActiveResultSets=True");

        private DbLocal(string dbName)
        {
            if (DbContextFactory == null)
            {
                throw new InvalidOperationException("Cannot create DbContextFactory");
            }

            CreateDbPath();

            CreateDb(dbName);
        }

        public void Dispose()
        {
            Instance.Value.Database.Connection.Close();
            Instance.Value.Database.Delete();
        }

        private void CreateDb(string dbName)
        {
            _connectionStringBuilder.AttachDBFilename = $@"|DataDirectory|\Db-{dbName}.mdf";
            Instance.Value = DbContextFactory(_connectionStringBuilder.ToString());
            Instance.Value.Database.CreateIfNotExists();
        }

        private static void CreateDbPath()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppData");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            const string datadirectory = "DataDirectory";

            if (AppDomain.CurrentDomain.GetData(datadirectory) == null)
            {
                AppDomain.CurrentDomain.SetData(datadirectory, path);
            }
        }

        public static DbLocal Create(Type context)
        {
            return new DbLocal(context.Name);
        }

        public static T GetTypedDbContext<T>() where T : DbContext
        {
            return Instance.Value as T;
        }
    }
}