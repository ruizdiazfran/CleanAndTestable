using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fixie;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Thing.Core.Infrastructure.Mapper;
using Fixture = Ploeh.AutoFixture.Fixture;

namespace Thing.Tests.Integration
{
    public abstract class FixieConventionBase : Convention
    {
        protected FixieConventionBase()
        {
            AutoMapperBootstrapper.Initialize();

            Classes
                .NameEndsWith(Constant.FixtureSuffix);

            Methods.Where(_ => _.IsVoid() || _.IsAsync());

            ClassExecution
                .CreateInstancePerClass()
                .UsingFactory(CustomCtorFactory);

            Parameters
                .Add(GetParameters);
        }

        internal static Lazy<Fixture> Fixture = new Lazy<Fixture>(()=> {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture;
        });

        private IEnumerable<object[]> GetParameters(MethodInfo method)
        {
            var parameterTypes = method.GetParameters().Select(x => x.ParameterType);

            var parameterValues = parameterTypes.Select(CustomParameterFactory).ToArray();

            return new[] {parameterValues};
        }

        protected virtual object CustomParameterFactory(Type t)
        {
            return new SpecimenContext(Fixture.Value).Resolve(t);
        }

        protected abstract object CustomCtorFactory(Type t);
    }
}