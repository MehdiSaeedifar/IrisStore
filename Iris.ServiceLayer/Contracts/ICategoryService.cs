using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Iris.DomainClasses;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer.Contracts
{
    public interface ICategoryService
    {
        void Add(Category category);
        Task<IList<Category>> GetListOfActualCategories(IList<string> categoriesList);
        Task<IList<Category>> GetAll();
        Task<IList<CategoryViewModel>> GetSearchProductsCategories();
        Task<IList<CategoryViewModel>> SearchCategory(string term, int count);
        Task<IList<SidebarCategoryViewModel>> GetSidebarCategories(int count);

        Task<DataGridViewModel<CategoryDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
             NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize);

        void Delete(int id);
        void Edit(Category category);

    }
}
