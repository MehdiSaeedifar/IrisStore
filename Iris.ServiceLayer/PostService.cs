using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer
{
    public class PostService : IPostService
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Post> _posts;

        public PostService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _posts = unitOfWork.Set<Post>();
            _mappingEngine = mappingEngine;
        }

        public async Task<DataGridViewModel<PostDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form, DateTimeType dateTimeType,
            int page, int pageSize)
        {
            var query = _posts.Where(p => !(p is Page)).AsQueryable();

            query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(query);

            var dataGridModel = new DataGridViewModel<PostDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<PostDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };

            return dataGridModel;
        }

        public void Add(Post post)
        {
            _posts.Add(post);
        }

        public void Delete(int id)
        {
            var entity = new Post() { Id = id };

            _unitOfWork.Entry(entity).State = EntityState.Deleted;
        }

        public void Edit(Post post)
        {
            _posts.Attach(post);
            _unitOfWork.Entry(post).State = EntityState.Modified;
            _unitOfWork.Entry(post).Property(p => p.ViewNumber).IsModified = false;
        }

        public async Task<AddPostViewModel> GetPostForEdit(int id)
        {
            return
                await
                    _posts.Where(p => p.Id == id)
                        .ProjectTo<AddPostViewModel>(null, _mappingEngine)
                        .FirstOrDefaultAsync();
        }

        public async Task<PostViewModel> GetPost(int id)
        {
            var selectedPost = await _posts.AsNoTracking().Where(post => post.Id == id)
                .ProjectTo<PostViewModel>(null, _mappingEngine).FirstOrDefaultAsync();

            var postEntity = new Post
            {
                Id = selectedPost.Id,
                ViewNumber = ++selectedPost.ViewNumber,
            };

            _posts.Attach(postEntity);
            _unitOfWork.Entry(postEntity).Property(p => p.ViewNumber).IsModified = true;

            return selectedPost;

        }

        public async Task IncrementViewNumber(int id)
        {
            //await _unitOfWork.Database.ExecuteSqlCommandAsync("UPDATE Posts SET ViewNumber = ViewNumber + 1 WHERE Id = @id ",
            //    new SqlParameter("@id", id));

            var post = await _posts.FindAsync(id);
            post.ViewNumber++;
        }


        public async Task<PagedListViewModel<PagedListPostViewModel>> GetPagedList(int categoryId, int pageNumber, int pageSize)
        {
            var resultsToSkip = pageNumber * pageSize;

            var pagedList = new PagedListViewModel<PagedListPostViewModel>
            {
                List = await _posts.Where(post => post.CategoryId == categoryId).OrderByDescending(post => post.PostedDate)
                    .ProjectTo<PagedListPostViewModel>(null, _mappingEngine)
                    .Skip(() => resultsToSkip)
                    .Take(() => pageSize)
                    .ToListAsync(),
                TotalCount = await _posts.CountAsync()
            };

            return pagedList;
        }

        public async Task<IList<LueneProduct>> GetAllForLuceneIndex()
        {
            return await _posts.AsNoTracking().Where(p => !(p is Page)).AsQueryable().Select(p => new LueneProduct
            {
                Id = p.Id,
                Title = p.Title,
                Image = p.Image,
                Description = p.Body,
                Category = p.Category.Name,
                SlugUrl = p.SlugUrl
            }).ToListAsync();
        }

        public async Task<int> GetTotalPostsCount()
        {
            return await _posts.CountAsync();
        }
    }
}
