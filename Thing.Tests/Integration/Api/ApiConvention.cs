using System;
using Thing.Core.Infrastructure.Mapper;

namespace Thing.Tests.Integration.Api
{
    public class ApiConvention : FixieConventionBase
    {
        public ApiConvention()
        {
            Classes.InTheSameNamespaceAs(typeof (ApiConvention));
        }

        protected override object CustomCtorFactory(Type t)
        {
            if (t.Name.EndsWith(Constant.FixtureSuffix))
            {
                return Activator.CreateInstance(t, TestStartup.CreateHttpClient());
            }

            throw new InvalidOperationException($"Cannot resolve {t.Name}");
        }
    }
}