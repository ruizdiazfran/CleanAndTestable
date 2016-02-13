using System;

namespace Thing.Tests.Integration.Db
{
    public class DbConvention : FixieConventionBase
    {
        public DbConvention()
        {
            Classes
                .InTheSameNamespaceAs(typeof (DbConvention));

            ClassExecution
                .Wrap<InitializeTestClass>();
        }

        protected override object CustomCtorFactory(Type t)
        {
            return ContainerLocal.Resolve(t);
        }
    }
}