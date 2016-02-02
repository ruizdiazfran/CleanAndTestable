using AutoMapper;
using SampleLibrary.Command;
using SampleLibrary.ViewModel;

namespace SampleLibrary.Infrastructure.Mapper
{
    public class ThingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Thing.Create, Domain.Thing>();
            CreateMap<Thing.Update, Domain.Thing>();
            CreateMap<ThingDetailViewModel, Domain.Thing>();
            CreateMap<Domain.Thing, ThingDetailViewModel>();
        }
    }
}