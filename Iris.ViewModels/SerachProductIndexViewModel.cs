using System.Collections.Generic;

namespace Iris.ViewModels
{
    #region SerachProductIndexViewModel
    public class SerachProductIndexViewModel
    {
        #region Properties
        public GroupsViewModel Categories { get; set; }
        public IList<decimal> Prices { get; set; }
        public IList<decimal> Discounts { get; set; }
        #endregion
    }
    #endregion
}
