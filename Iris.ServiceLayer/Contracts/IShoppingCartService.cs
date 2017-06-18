using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer.Contracts
{
    public interface IShoppingCartService
    {
        Task CreateFactor(CreateFactorViewModel factorViewModel);
        Task<IList<ListFactorViewModel>> GetUserFactor(int userId);

        Task<DataGridViewModel<FactorDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
            NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize);

        void Delete(int id);
        Task<ListFactorViewModel> GetForEdit(int id);
        Task Edit(ListFactorViewModel factorViewModel);
    }

    
}
