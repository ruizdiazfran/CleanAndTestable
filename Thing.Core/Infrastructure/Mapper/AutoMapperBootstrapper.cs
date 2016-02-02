using System;
using System.Linq;
using AutoMapper;

namespace SampleLibrary.Infrastructure.Mapper
{
    public class AutoMapperBootstrapper
    {
        private static readonly Lazy<AutoMapperBootstrapper> Bootstrapper =
            new Lazy<AutoMapperBootstrapper>(InternalInitialize);

        private AutoMapperBootstrapper()
        {
        }

        public static void Initialize()
        {
            var bootstrapper = Bootstrapper.Value;
        }

        private static AutoMapperBootstrapper InternalInitialize()
        {
            var profiles = typeof (AutoMapperBootstrapper)
                .Assembly
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof (Profile)))
                .Select(Activator.CreateInstance)
                .Cast<Profile>()
                .ToArray();

#pragma warning disable CS0618 // Type or member is obsolete
            AutoMapper.Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                    cfg.AddProfile(profile);
            });
#pragma warning restore CS0618 // Type or member is obsolete

            return new AutoMapperBootstrapper();
        }
    }
}