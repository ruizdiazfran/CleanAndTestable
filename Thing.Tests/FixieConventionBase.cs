using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fixie;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Thing.Tests.Utils;
using Fixture = Ploeh.AutoFixture.Fixture;

namespace Thing.Tests
{
    public abstract class FixieConventionBase : Convention
    {
        private readonly IFixture _fixture = new Fixture();

        protected FixieConventionBase()
        {
            Classes
                .NameEndsWith(Constant.FixtureSuffix);

            Methods.Where(_ => _.IsVoid() || _.IsAsync());

            ClassExecution
                .CreateInstancePerClass()
                .UsingFactory(CustomCtorFactory);

            Parameters
                .Add(GetParameters);
        }

        private IEnumerable<object[]> GetParameters(MethodInfo method)
        {
            var parameterTypes = method.GetParameters().Select(x => x.ParameterType);

            var parameterValues = parameterTypes.Select(CustomParameterFactory).ToArray();

            return new[] {parameterValues};
        }

        protected virtual object CustomParameterFactory(Type t)
        {
            return new SpecimenContext(_fixture).Resolve(t);
        }


        protected virtual object CustomCtorFactory(Type t)
        {
            return new SpecimenContext(_fixture).Resolve(t);
        }
    }    
}