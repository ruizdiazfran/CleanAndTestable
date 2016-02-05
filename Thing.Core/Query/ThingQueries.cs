using MediatR;
using Thing.Core.ViewModel;

namespace Thing.Core.Query
{
    public class ThingQuery
    {
        public class GetAll : IAsyncRequest<ThingAllViewModel>
        {
        }

        public class GetByName : IAsyncRequest<ThingDetailViewModel>
        {
            public string Name { get; set; }
        }

        public class GetById : IAsyncRequest<ThingDetailViewModel>
        {
            public string Id { get; set; }
        }
    }
}