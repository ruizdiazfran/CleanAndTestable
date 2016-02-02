using System.Collections.Generic;

namespace SampleLibrary.ViewModel
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