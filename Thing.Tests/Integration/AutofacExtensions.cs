using Autofac;

namespace Thing.Tests.Integration
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterTests(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof (AutofacExtensions).Assembly)
                .Where(_ => _.Name.EndsWith(Constant.FixtureSuffix));

            return builder;
        }
    }
}