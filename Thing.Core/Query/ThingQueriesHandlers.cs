using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SampleLibrary.Contracts;
using SampleLibrary.ViewModel;

namespace SampleLibrary.Query
{
    public class ThingQueriesHandlers : IAsyncRequestHandler<Query.GetAll, ThingAllViewModel>,
        IAsyncRequestHandler<Query.GetByName, ThingDetailViewModel>,
        IAsyncRequestHandler<Query.GetById, ThingDetailViewModel>
    {
        private readonly IThingRepository _thingRepository;

        public ThingQueriesHandlers(IThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }

        async Task<ThingAllViewModel> IAsyncRequestHandler<Query.GetAll, ThingAllViewModel>.Handle(Query.GetAll message)
        {
            var entities = await _thingRepository.GetAllAsync();
#pragma warning disable 618
            return new ThingAllViewModel(Mapper.Map<IEnumerable<ThingDetailViewModel>>(entities));
#pragma warning restore 618
        }

        async Task<ThingDetailViewModel> IAsyncRequestHandler<Query.GetById, ThingDetailViewModel>.Handle(
            Query.GetById message)
        {
            var entity = await _thingRepository.GetByIdAsync(message.Id);
#pragma warning disable 618
            return Mapper.Map<ThingDetailViewModel>(entity);
#pragma warning restore 618
        }

        async Task<ThingDetailViewModel> IAsyncRequestHandler<Query.GetByName, ThingDetailViewModel>.Handle(
            Query.GetByName message)
        {
            var entity = await _thingRepository.FindByNameAsync(message.Name);
#pragma warning disable 618
            return Mapper.Map<ThingDetailViewModel>(entity);
#pragma warning restore 618
        }
    }
}