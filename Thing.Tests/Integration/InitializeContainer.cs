using System;
using System.Diagnostics;
using Fixie;
using Thing.Tests.Utils;

namespace Thing.Tests.Integration
{
    internal class InitializeContainer : ClassBehavior
    {
        public void Execute(Class context, Action next)
        {
            ContainerLocal.Begin();
            Debug.WriteLine("Start " + nameof(InitializeContainer));
            next();
            Debug.WriteLine("End " + nameof(InitializeContainer));
            ContainerLocal.End();
        }
    }
}