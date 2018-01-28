﻿using System.Collections.Generic;
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
    public class PostCategoryService : IPostCategoryService
    {
        #region Fileds
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<PostCategory> _postCategories;
        private readonly IMappingEngine _mappingEngine;
        #endregion

        #region Constractors
        public PostCategoryService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _postCategories = unitOfWork.Set<PostCategory>();
            _mappingEngine = mappingEngine;
        }
        #endregion

        #region GetDataGridSource
        public async Task<DataGridViewModel<CategoryDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form, DateTimeType dateTimeType,
            int page, int pageSize)
        {
            var query = _postCategories.AsQueryable();

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
        #endregion

        #region Add
        public void Add(PostCategory category)
        {
            _postCategories.Add(category);
        }
        #endregion

        #region Delete
        public void Delete(int id)
        {
            var entity = new PostCategory { Id = id };
            _postCategories.Attach(entity);
            _postCategories.Remove(entity);
        }
        #endregion

        #region Edit
        public void Edit(PostCategory category)
        {
            _postCategories.Attach(category);

            _unitOfWork.Entry(category).State = EntityState.Modified;
        }
        #endregion

        #region GetCategoryLinks
        public async Task<IList<LinkViewModel>> GetCategoryLinks()
        {
            return await _postCategories.AsNoTracking().OrderBy(pc => pc.Order)
                .ProjectTo<LinkViewModel>(null, _mappingEngine).Cacheable()
                .ToListAsync();
        }
        #endregion

        #region GetAll
        public async Task<IList<CategoryViewModel>> GetAll()
        {
            return await _postCategories.OrderBy(pc => pc.Name).ProjectTo<CategoryViewModel>(null, _mappingEngine)
                .ToListAsync();
        }
        #endregion

        #region GetSideBar
        public async Task<IList<PostCategorySideBarViewModel>> GetSideBar()
        {
            return await _postCategories.AsNoTracking().OrderBy(pc => pc.Order).Where(pc => pc.Posts.Count > 0)
                .Select(pc => new PostCategorySideBarViewModel
                {
                    Id = pc.Id,
                    Name = pc.Name,
                    Posts = pc.Posts
                    .OrderByDescending(post => post.PostedDate)
                    .Select(post => new LinkViewModel { Id = post.Id, Title = post.Title, SlugUrl = post.SlugUrl, Image = post.Image })
                    .ToList()
                })
                .Cacheable()
                .ToListAsync();
        }
        #endregion

        #region GetCategoryName
        public async Task<string> GetCategoryName(int id)
        {
            return await _postCategories
                .Where(pc => pc.Id == id).Select(pc => pc.Name).FirstOrDefaultAsync();
        }
        #endregion
    }
}
