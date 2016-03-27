using System.Collections.Generic;

namespace Iris.ViewModels
{
    public class DataGridViewModel<T>
    {
        public List<T> Records { get; set; }
        public int TotalCount { get; set; }
    }
}
