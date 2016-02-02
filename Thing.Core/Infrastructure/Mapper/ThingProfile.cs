using AutoMapper;
using SampleLibrary.Domain;
using SampleLibrary.ViewModel;

namespace SampleLibrary.Infrastructure.Mapper
{
    public class ThingProfile : Profile
    {
        protected override void Configure()
        {
            // ViewModel
            CreateMap<ThingDetailViewModel, Thing>();
            CreateMap<Thing, ThingDetailViewModel>()
                .ForMember(x => x.AddressLine, x => x.MapFrom(_ => _.Address.Line))
                .ForMember(x => x.AddressZip, x => x.MapFrom(_ => _.Address.Zip));
        }
    }
}