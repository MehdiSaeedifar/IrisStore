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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<Category> _categories;

        public CategoryService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _categories = unitOfWork.Set<Category>();
            _mappingEngine = mappingEngine;
        }

        public void Add(Category category)
        {
            _categories.Add(category);
        }

        public async Task<IList<Category>> GetListOfActualCategories(IList<string> categoriesList)
        {
            return await _categories.Where(x => categoriesList.Contains(x.Name)).ToListAsync();
        }

        public async Task<IList<Category>> GetAll()
        {
            return await _categories.ToListAsync();
        }

        public async Task<IList<CategoryViewModel>> GetSearchProductsCategories()
        {
            return await _categories.OrderByDescending(c => c.Products.Count)
                .ProjectTo<CategoryViewModel>(parameters: null, mappingEngine: _mappingEngine)
                .Cacheable().ToListAsync();

        }

        public async Task<IList<CategoryViewModel>> SearchCategory(string term, int count)
        {
            return await _categories.OrderBy(category => category.Name)
                .Where(category => category.Name.Contains(term))
                .ProjectTo<CategoryViewModel>(null, _mappingEngine).Take(count).ToListAsync();
        }

        public async Task<IList<SidebarCategoryViewModel>> GetSidebarCategories(int count)
        {
            return await _categories.AsNoTracking()
                        .OrderByDescending(category => category.Products.Count)
                        .ProjectTo<SidebarCategoryViewModel>(null, _mappingEngine)
                        .Take(count)
                        .Cacheable().ToListAsync();
        }

        public async Task<DataGridViewModel<CategoryDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form, DateTimeType dateTimeType,
            int page, int pageSize)
        {
            var query = _categories.AsQueryable();

            query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(query);

            var dataGridModel = new DataGridViewModel<CategoryDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<CategoryDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };

            return dataGridModel;
        }

        public void Delete(int id)
        {
            var entity = new Category { Id = id };
            _categories.Attach(entity);
            _categories.Remove(entity);
        }

        public void Edit(Category category)
        {
            _categories.Attach(category);

            _unitOfWork.Entry(category).State = EntityState.Modified;
        }
    }
}
