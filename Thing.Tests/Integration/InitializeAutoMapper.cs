using System;
using System.Diagnostics;
using Fixie;
using Thing.Core.Infrastructure.Mapper;

namespace Thing.Tests.Integration
{
    internal class InitializeAutoMapper : ClassBehavior
    {
        public void Execute(Class context, Action next)
        {
            Debug.WriteLine("Start " + nameof(InitializeAutoMapper));
            AutoMapperBootstrapper.Initialize();

            next();
            Debug.WriteLine("End " + nameof(InitializeAutoMapper));
        }
    }
}