using System;
using System.Diagnostics;
using Fixie;
using Thing.Tests.Utils;

namespace Thing.Tests.Integration
{
    internal class InitializeDatabase : ClassBehavior
    {
        public void Execute(Class context, Action next)
        {
            using (DbLocal.Create(context.Type))
            {
                Debug.WriteLine("Start " + nameof(InitializeDatabase));
                next();
                Debug.WriteLine("End " + nameof(InitializeDatabase));
            }
        }
    }
}