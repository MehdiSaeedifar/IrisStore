using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Iris.DomainClasses;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer.Contracts
{
    public interface IPostService
    {
        Task<DataGridViewModel<PostDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
               NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize);

        void Add(Post post);
        void Delete(int id);
        void Edit(Post post);
        Task<AddPostViewModel> GetPostForEdit(int id);
        Task<PostViewModel> GetPost(int id);
        Task IncrementViewNumber(int id);
        Task<PagedListViewModel<PagedListPostViewModel>> GetPagedList(int categoryId,int pageNumber, int pageSize);
        Task<IList<LueneProduct>> GetAllForLuceneIndex();
        Task<int> GetTotalPostsCount();
    }
}
