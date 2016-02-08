using System;
using Autofac;
using Fixie;
using Thing.Api.Infrastructure;
using Thing.Core.Infrastructure.Persistence;
using Thing.Tests.Integration.Utils;

namespace Thing.Tests.Integration.Db
{
    public class DbConvention : Convention
    {
        static DbConvention()
        {
            DbLocal.DbContextFactory = CreateDbContext;
            CompositionRoot.Override = ReplaceServices;
        }

        public DbConvention()
        {
            Classes.InTheSameNamespaceAs(typeof (DbConvention))
                .Where(_ => _.Name.EndsWith(Constant.FixtureSuffix));

            Methods.Where(_ => _.IsVoid() || _.IsAsync());

            ClassExecution
                .Wrap<InitializeDatabase>()
                .Wrap<InitializeContainer>()
                .Wrap<InitializeAutoMapper>()
                .UsingFactory(ContainerLocal.Resolve);
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