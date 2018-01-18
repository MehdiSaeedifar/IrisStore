using System;
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
    public class ProductService : IProductService
    {
        private readonly IDbSet<Product> _products;
        private readonly IDbSet<Category> _categories;
        private readonly IDbSet<ProductImage> _productImages;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;

        public ProductService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _products = unitOfWork.Set<Product>();
            _categories = unitOfWork.Set<Category>();
            _productImages = unitOfWork.Set<ProductImage>();
            _mappingEngine = mappingEngine;
        }

        public async Task AddProduct(Product product)
        {

            var selectedCategoryNamesList = product.Categories.Select(c => c.Name).ToList();


            product.Categories.Clear();

            ////سپس در طی فقط یک کوئری بررسی می‌کنیم کدامیک از موارد ارسالی موجود هستند
            var listOfActualCategories = await _categories.Where(x => selectedCategoryNamesList.Contains(x.Name)).ToListAsync();
            var listOfActualCategoryNames = listOfActualCategories.Select(x => x.Name.ToLower()).ToList();

            //فقط موارد جدید به تگ‌ها و ارتباطات موجود اضافه می‌شوند
            foreach (var tag in selectedCategoryNamesList)
            {
                if (!listOfActualCategoryNames.Contains(tag.ToLowerInvariant().Trim()))
                {
                    product.Categories.Add(new Category { Name = tag.Trim() });
                }
            }

            //موارد قبلی هم حفظ می‌شوند
            foreach (var item in listOfActualCategories)
            {
                product.Categories.Add(item);
            }

            _products.Add(product);
        }

        public async Task<IList<ProductImage>> EditProduct(Product editedProduct)
        {
            var selectedProduct = await _products
                .Include(p => p.Categories)
                .Include(p => p.Images)
                .Include(p => p.Prices)
                .Include(p => p.Discounts)
                .Where(p => p.Id == editedProduct.Id).SingleOrDefaultAsync();

            //_products.Attach(editedProduct);
            //_unitOfWork.MarkAsChanged(editedProduct);

            editedProduct.EditedDate = DateTime.Now;
            _unitOfWork.Entry(selectedProduct).CurrentValues.SetValues(editedProduct);
            _unitOfWork.Entry(selectedProduct).Property(p => p.PostedDate).IsModified = false;
            _unitOfWork.Entry(selectedProduct).Property(p => p.PostedByUserId).IsModified = false;
            _unitOfWork.Entry(selectedProduct).Property(p => p.AverageRating).IsModified = false;
            _unitOfWork.Entry(selectedProduct).Property(p => p.TotalRaters).IsModified = false;
            _unitOfWork.Entry(selectedProduct).Property(p => p.TotalRating).IsModified = false;
            _unitOfWork.Entry(selectedProduct).Property(p => p.ViewNumber).IsModified = false;

            var existingImages = selectedProduct.Images.ToList();
            var deletedImages = UpdateOneToManyRelation(editedProduct.Images.ToList(), existingImages);

            var existingPrices = selectedProduct.Prices.ToList();
            UpdateOneToManyRelation(editedProduct.Prices.ToList(), existingPrices);

            var existingDiscounts = selectedProduct.Discounts.ToList();
            UpdateOneToManyRelation(editedProduct.Discounts.ToList(), existingDiscounts);

            //ابتدا کلیه گروه‌های موجود را حذف خواهیم کرد
            if (selectedProduct.Categories != null && selectedProduct.Categories.Any())
                selectedProduct.Categories.Clear();

            var updatedCategoryNames = editedProduct.Categories.Select(p => p.Name).ToList();

            //سپس در طی فقط یک کوئری بررسی می‌کنیم کدامیک از موارد ارسالی موجود هستند
            var listOfActualTags = _categories.Where(x => updatedCategoryNames.Contains(x.Name)).ToList();
            var listOfActualTagNames = listOfActualTags.Select(x => x.Name.ToLower()).ToList();

            //فقط موارد جدید به تگ‌ها و ارتباطات موجود اضافه می‌شوند
            foreach (var tag in updatedCategoryNames)
            {
                if (!listOfActualTagNames.Contains(tag.ToLowerInvariant().Trim()))
                {
                    var newCategory = _categories.Add(new Category() { Name = tag.Trim() });
                    selectedProduct.Categories.Add(newCategory);
                }
            }

            //موارد قبلی هم حفظ می‌شوند
            foreach (var item in listOfActualTags)
            {
                selectedProduct.Categories.Add(item);
            }

            return deletedImages;
        }



        private IList<T> UpdateOneToManyRelation<T>(List<T> updatedList, List<T> existingList) where T : BaseEntity
        {
            var addedEntities = updatedList.Where(x => existingList.All(item => item.Id != x.Id)).ToList();

            var deletedEntities = existingList.Where(x => updatedList.All(item => item.Id != x.Id)).ToList();

            var modifiedEntities = updatedList.Where(x => addedEntities.All(item => item.Id != x.Id)).ToList();

            addedEntities.ForEach(entity => _unitOfWork.MarkAsAdded(entity));

            deletedEntities.ForEach(entity => _unitOfWork.MarkAsDeleted(entity));

            foreach (var entity in modifiedEntities)
            {
                var existingEntity = _unitOfWork.Set<T>().Find(entity.Id);

                if (existingEntity != null)
                {
                    var entityEntry = _unitOfWork.Entry(existingEntity);
                    entityEntry.CurrentValues.SetValues(entity);
                }

            }
            return deletedEntities;
        }

        public async Task<AddProductViewModel> GetProductForEdit(int productId)
        {
            return await _products.Where(p => p.Id == productId)
                .ProjectTo<AddProductViewModel>(parameters: null, mappingEngine: _mappingEngine).SingleOrDefaultAsync();
        }

        public async Task<DataGridViewModel<ProductDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize)
        {
            var usersQuery = _products.AsQueryable();

            var query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(usersQuery);

            var dataGridModel = new DataGridViewModel<ProductDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<ProductDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };
            return dataGridModel;
        }

        public void DeleteProduct(int productId)
        {
            var entity = new Product() { Id = productId };
            _unitOfWork.Entry(entity).State = EntityState.Deleted;
        }

        public async Task<IList<ProductWidgetViewModel>> GetNewestProducts(int count)
        {
            return await _products.AsNoTracking().OrderBy(product => product.ProductStatus).ThenByDescending(product => product.PostedDate)
                .Take(count)
                             .ProjectTo<ProductWidgetViewModel>(parameters: null, mappingEngine: _mappingEngine)
                                .Cacheable().ToListAsync();
        }

        public async Task<IList<ProductWidgetViewModel>> GetMostViewedProducts(int count)
        {
            return await _products.AsNoTracking().OrderBy(product => product.ProductStatus).ThenByDescending(product => product.ViewNumber)
                .Take(count)
                           .ProjectTo<ProductWidgetViewModel>(parameters: null, mappingEngine: _mappingEngine)
                              .Cacheable().ToListAsync();
        }

        public async Task<IList<ProductWidgetViewModel>> GetPopularProducts(int count)
        {
            return await _products.AsNoTracking().OrderByDescending(product => product.AverageRating)
                          .Take(count)
                          .ProjectTo<ProductWidgetViewModel>(parameters: null, mappingEngine: _mappingEngine)
                             .Cacheable().ToListAsync();
        }

        public async Task<IList<decimal>> GetAvailableProductPrices()
        {
            return await _products.Where(product => product.ProductStatus == ProductStatus.Available)
                .Select(product => product.Prices
                        .OrderByDescending(price => price.Date).Select(price => price.Price)
                        .FirstOrDefault())
                .OrderBy(price => price)
                .ToListAsync();
        }

        public async Task<IList<decimal>> GetAvailableProductDiscounts()
        {
            return await _products.Where(product => product.ProductStatus == ProductStatus.Available)
                .Select(product => product.Discounts
                        .OrderByDescending(discount => discount.StartDate).Select(discount => discount.Discount)
                        .FirstOrDefault())
                .OrderBy(discount => discount)
                .ToListAsync();
        }

        public async Task<ProductSearchPagedList> SearchProduct(SearchProductViewModel searchModel)
        {
            var productsQuery = _products.AsQueryable();

            if (searchModel.SelectedCategories != null && searchModel.SelectedCategories.Any())
            {
                productsQuery =
                    productsQuery.Where(product =>
                    product.Categories.Any(category => searchModel.SelectedCategories.Contains(category.Id)));
            }

            productsQuery = productsQuery.OrderBy($"ProductStatus,{searchModel.SortBy} {searchModel.SortOrder}");


            if (searchModel.ShowStockProductsOnly)
            {
                productsQuery = productsQuery.Where(product => product.ProductStatus == ProductStatus.Available);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
            {
                productsQuery = productsQuery.Where("Title.Contains(@0)", searchModel.SearchTerm);
            }


            if (searchModel.MinPrice.HasValue || searchModel.MaxPrice.HasValue)
            {
                productsQuery = productsQuery
                .Where(product => product.Prices.OrderByDescending(price => price.Date).Select(price => price.Price).FirstOrDefault() >= searchModel.MinPrice &&
                product.Prices.OrderByDescending(price => price.Date).Select(price => price.Price).FirstOrDefault() <= searchModel.MaxPrice);
            }

            if (searchModel.MinDiscount.HasValue || searchModel.MaxDiscount.HasValue)
            {
                productsQuery = productsQuery
                .Where(product => product.Discounts.OrderByDescending(discount => discount.StartDate).Select(discount => discount.Discount).FirstOrDefault() >= searchModel.MinDiscount &&
                product.Discounts.OrderByDescending(discount => discount.StartDate).Select(discount => discount.Discount).FirstOrDefault() <= searchModel.MaxDiscount);
            }

            var result = new ProductSearchPagedList
            {
                Products = await productsQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
                    .Take(searchModel.PageSize)
                    .ProjectTo<ProductWidgetViewModel>(null, _mappingEngine).ToListAsync(),
                TotalCount = await productsQuery.CountAsync()
            };



            return result;
        }

        public async Task<ProductPageViewModel> GetProductPage(int productId)
        {
            var selectedProduct = await _products.Where(product => product.Id == productId)
                        .ProjectTo<ProductPageViewModel>(null, _mappingEngine)
                        .FirstOrDefaultAsync();

            UpdateViewNumber(selectedProduct);

            return selectedProduct;
        }

        public async Task UpdateViewNumber(int productId)
        {
            //await _unitOfWork.Database.ExecuteSqlCommandAsync("UPDATE Products SET ViewNumber = ViewNumber + 1 WHERE Id = @productId ", new SqlParameter("@productId", productId));
            var product = await _products.FindAsync(productId);
            product.ViewNumber++;
        }

        private void UpdateViewNumber(ProductPageViewModel product)
        {
            var postEntity = new Product
            {
                Id = product.Id,
                ViewNumber = ++product.ViewNumber,
            };

            _products.Attach(postEntity);
            _unitOfWork.Entry(postEntity).Property(p => p.ViewNumber).IsModified = true;
        }

        public async Task SaveRating(int productId, double rating)
        {
            var selectedProduct = await _products.FindAsync(productId);

            if (!selectedProduct.TotalRaters.HasValue)
                selectedProduct.TotalRaters = 0;
            if (!selectedProduct.TotalRating.HasValue)
                selectedProduct.TotalRating = 0;
            if (!selectedProduct.AverageRating.HasValue)
                selectedProduct.AverageRating = 0;

            selectedProduct.TotalRaters++;
            selectedProduct.TotalRating += rating;
            selectedProduct.AverageRating = selectedProduct.TotalRating / selectedProduct.TotalRaters;

        }

        public async Task<IList<LueneProduct>> GetAllForLuceneIndex()
        {
            return await _products.AsNoTracking().Select(p => new LueneProduct
            {
                Id = p.Id,
                ProductStatus = p.ProductStatus,
                Price = p.Prices.OrderByDescending(price => price.Date).Select(price => price.Price).FirstOrDefault(),
                Discount = p.Discounts.OrderByDescending(discount => discount.StartDate).Select(discount => discount.Discount).FirstOrDefault(),
                Title = p.Title,
                Image = p.Images.OrderBy(image => image.Order).Select(image => image.ThumbnailUrl).FirstOrDefault(),
                Description = p.Body,
                SlugUrl = p.SlugUrl
            }).ToListAsync();
        }

        public async Task<IList<string>> GetProductImages(int productId)
        {
            return await _productImages.Where(pi => pi.ProductId == productId)
                .Select(pi => pi.Name).ToListAsync();
        }

        public async Task<IList<ProductOrderViewModel>> GetProductsOrders(int[] productIds)
        {
            return await _products.Where(p => productIds.Contains(p.Id))
                                    .ProjectTo<ProductOrderViewModel>()
                                    .ToListAsync();
        }
    }

}
