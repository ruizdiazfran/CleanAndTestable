using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SampleLibrary.Contracts;
using SampleLibrary.ViewModel;

namespace SampleLibrary.Query
{
    public class ThingQueriesHandlers : IAsyncRequestHandler<ThingQuery.GetAll, ThingAllViewModel>,
        IAsyncRequestHandler<ThingQuery.GetByName, ThingDetailViewModel>,
        IAsyncRequestHandler<ThingQuery.GetById, ThingDetailViewModel>
    {
        private readonly IThingRepository _thingRepository;

        public ThingQueriesHandlers(IThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }

        public async Task<ThingAllViewModel> Handle(
            ThingQuery.GetAll message)
        {
            var entities = await _thingRepository.GetAllAsync();
#pragma warning disable 618
            return new ThingAllViewModel(Mapper.Map<IEnumerable<ThingDetailViewModel>>(entities));
#pragma warning restore 618
        }

        public async Task<ThingDetailViewModel> Handle(
            ThingQuery.GetById message)
        {
            var entity = await _thingRepository.GetByIdAsync(message.Id);
#pragma warning disable 618
            return Mapper.Map<ThingDetailViewModel>(entity);
#pragma warning restore 618
        }

        public async Task<ThingDetailViewModel> Handle(
            ThingQuery.GetByName message)
        {
            var entity = await _thingRepository.FindByNameAsync(message.Name);
#pragma warning disable 618
            return Mapper.Map<ThingDetailViewModel>(entity);
#pragma warning restore 618
        }
    }
}