using System.Collections.Generic;

namespace Iris.ViewModels
{
    public class SerachProductIndexViewModel
    {
        public GroupsViewModel Categories { get; set; }
        public IList<decimal> Prices { get; set; }
    }
}
