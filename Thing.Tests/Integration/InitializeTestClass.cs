using System;
using System.Diagnostics;
using Fixie;
using Thing.Core.Infrastructure.Mapper;

namespace Thing.Tests.Integration
{
    internal class InitializeTestClass : ClassBehavior
    {
        public void Execute(Class context, Action next)
        {
            AutoMapperBootstrapper.Initialize();

            using (ContainerLocal.Create())
            {
                Debug.WriteLine("   Start " + nameof(InitializeTestClass));
                next();
                Debug.WriteLine("   End " + nameof(InitializeTestClass));
            }
        }
    }
}