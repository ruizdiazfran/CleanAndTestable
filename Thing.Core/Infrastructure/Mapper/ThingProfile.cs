using AutoMapper;
using Thing.Core.ViewModel;

namespace Thing.Core.Infrastructure.Mapper
{
    public class ThingProfile : Profile
    {
        protected override void Configure()
        {
            // ViewModel
            CreateMap<ThingDetailViewModel, Domain.Thing>();
            CreateMap<Domain.Thing, ThingDetailViewModel>()
                .ForMember(x => x.AddressLine, x => x.MapFrom(_ => _.Address.Line))
                .ForMember(x => x.AddressZip, x => x.MapFrom(_ => _.Address.Zip));
        }
    }
}