using System.Collections.Generic;

namespace Thing.Core.ViewModel
{
    public class ThingAllViewModel : List<ThingDetailViewModel>
    {
        public ThingAllViewModel()
        {
        }

        public ThingAllViewModel(IEnumerable<ThingDetailViewModel> collection) : base(collection)
        {
        }
    }
}