using System;
using Autofac;
using Ploeh.AutoFixture;
using Thing.Api.Infrastructure;
using Thing.Core.Infrastructure.Persistence;
using Thing.Tests.Utils;

namespace Thing.Tests.Integration.Db
{
    public class DbConvention : FixieConventionBase
    {
        static DbConvention()
        {
            DbLocal.DbContextFactory = CreateDbContext;
            CompositionRoot.Override = ReplaceServices;
        }

        public DbConvention()
        {
            Classes.InTheSameNamespaceAs(typeof (DbConvention));

            ClassExecution
                .Wrap<InitializeDatabase>()
                .Wrap<InitializeContainer>()
                .Wrap<InitializeAutoMapper>();
        }

        protected override object CustomCtorFactory(Type t)
        {
            return ContainerLocal.Resolve(t);
        }

        private static ThingDbContext CreateDbContext(string connectionString)
        {
            var dbContext = new ThingDbContext(connectionString);
            dbContext.Database.Log = Console.WriteLine;
            new DefaultDbInitializer().InitializeDatabase(dbContext);
            return dbContext;
        }

        private static void ReplaceServices(IContainer container)
        {
            var cb = new ContainerBuilder();
            //  register test
            cb.RegisterAssemblyTypes(typeof (DbConvention).Assembly)
                .Where(_ => _.Name.EndsWith(Constant.FixtureSuffix));
            //  inject local DbContext
            cb.Register(_ => DbLocal.GetTypedDbContext<ThingDbContext>())
                .AsSelf();
            //  update container
            cb.Update(container);
        }
    }
}