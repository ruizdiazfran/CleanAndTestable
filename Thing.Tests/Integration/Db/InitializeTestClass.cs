using System;
using System.Diagnostics;
using Fixie;

namespace Thing.Tests.Integration.Db
{
    internal class InitializeTestClass : ClassBehavior
    {
        public void Execute(Class context, Action next)
        {
            using (ContainerLocal.Create())
            {
                Debug.WriteLine("   Start " + nameof(InitializeTestClass));
                next();
                Debug.WriteLine("   End " + nameof(InitializeTestClass));
            }
        }
    }
}