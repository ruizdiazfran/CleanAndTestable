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
            DbLocal.DbContextFactory = c => new ThingDbContext(c);
            CompositionRoot.Override = Replace;
        }

        public DbConvention()
        {
            Classes.InTheSameNamespaceAs(typeof (DbConvention))
                .Where(_ => _.Name.EndsWith("Tests"));

            Methods.Where(_ => _.IsVoid() || _.IsAsync());

            ClassExecution
                .Wrap<InitializeDatabase>()
                .Wrap<InitializeContainer>()
                .Wrap<InitializeAutoMapper>()
                .UsingFactory(ContainerLocal.Resolve);

            CaseExecution
                .Wrap<RespawnDbData>();
        }

        private static void Replace(IContainer container)
        {
            var cb = new ContainerBuilder();
            cb.RegisterAssemblyTypes(typeof (DbConvention).Assembly)
                .Where(_ => _.Name.EndsWith("Tests"));
            cb.Register(_ => DbLocal.GetTypedDbContext<ThingDbContext>()).AsSelf();
            cb.Update(container);
        }
    }
}