using System.Collections.Generic;

namespace Iris.ViewModels
{
    #region DataGridViewModel<T>
    public class DataGridViewModel<T>
    {
        public List<T> Records { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion
}
