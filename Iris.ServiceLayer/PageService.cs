using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer
{
    public class PageService : IPageService
    {
        #region Fields
        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Page> _pages;
        #endregion

        #region Constractors
        public PageService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _pages = unitOfWork.Set<Page>();
            _mappingEngine = mappingEngine;
        }
        #endregion

        #region GetDataGridSource
        public async Task<DataGridViewModel<PageDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form, DateTimeType dateTimeType,
            int page, int pageSize)
        {
            var query = _pages.AsQueryable();

            query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(query);

            var dataGridModel = new DataGridViewModel<PageDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<PageDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };

            return dataGridModel;
        }
        #endregion

        #region FixOrder
        private void FixOrder(IList<Page> otherPages)
        {
            foreach (var page in otherPages)
            {
                _pages.Attach(page);
                _unitOfWork.Entry(page).Property(p => p.Order).IsModified = true;
            }
        }
        #endregion

        #region AddPage
        public void AddPage(Page page, IList<Page> otherPages)
        {
            _pages.Add(page);

            FixOrder(otherPages);
        }
        #endregion

        #region DeletePage
        public void DeletePage(int pageId)
        {
            var entity = new Page() { Id = pageId };

            _unitOfWork.Entry(entity).State = EntityState.Deleted;
        }
        #endregion

        #region EditPage
        public void EditPage(Page page, IList<Page> otherPages)
        {
            _pages.Attach(page);
            _unitOfWork.Entry(page).State = EntityState.Modified;
            _unitOfWork.Entry(page).Property(p => p.ViewNumber).IsModified = false;

            FixOrder(otherPages);
        }
        #endregion

        #region GetAllPagesForAdd
        public async Task<IList<AddPageViewModel>> GetAllPagesForAdd()
        {
            return await _pages
                         .OrderBy(page => page.Order).ThenByDescending(page => page.PostedDate)
                             .Select(page => new AddPageViewModel
                             {
                                 Id = page.Id,
                                 Title = page.Title,
                                 Order = page.Order
                             }).ToListAsync();
        }
        #endregion

        #region GetPage
        public async Task<AddPageViewModel> GetPage(int pageId)
        {
            return await _pages.Where(page => page.Id == pageId)
               .ProjectTo<AddPageViewModel>().FirstOrDefaultAsync();
        }
        #endregion

        #region GetPageLinks
        public async Task<IList<LinkViewModel>> GetPageLinks()
        {
            return await _pages.OfType<Page>().AsNoTracking().OrderBy(p => p.Order).ProjectTo<LinkViewModel>(null, _mappingEngine)
                .Cacheable()
                .ToListAsync();
        }
        #endregion

        #region GetResumePage
        public async Task<PostViewModel> GetResumePage(string title)
        {
            var selectedPage = await _pages.Where(post => post.Title == title)
                 .ProjectTo<PostViewModel>(null, _mappingEngine).FirstOrDefaultAsync();

            var postEntity = new Page
            {
                Id = selectedPage.Id,
                ViewNumber = ++selectedPage.ViewNumber,
            };

            _pages.Attach(postEntity);
            _unitOfWork.Entry(postEntity).Property(p => p.ViewNumber).IsModified = true;

            return selectedPage;
        }
        #endregion
    }
}
