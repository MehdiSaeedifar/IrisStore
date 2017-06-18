using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using Microsoft.AspNet.Identity;
using System.Linq.Dynamic;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Iris.ServiceLayer
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<Factor> _factors;
        private IShoppingCartService _shoppingCartServiceImplementation;

        public ShoppingCartService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _factors = unitOfWork.Set<Factor>();
        }

        public async Task CreateFactor(CreateFactorViewModel factorViewModel)
        {
            var factor = new Factor
            {
                Name = factorViewModel.Name,
                Address = factorViewModel.Address,
                UserId = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId()),
                PhoneNumber = factorViewModel.PhoneNumber,
                LastName = factorViewModel.LastName,
                IssueDate = DateTime.Now
            };


            var factorProducts = new List<FactorProduct>();

            foreach (var factorProduct in factorViewModel.Products)
            {
                var selectedProduct = await _unitOfWork.Set<Product>().FindAsync(factorProduct.ProductId);

                factorProducts.Add(new FactorProduct
                {
                    Count = factorProduct.Count,
                    Price =selectedProduct.Prices.OrderByDescending(x=>x.Date).FirstOrDefault().Price,
                    ProductId = selectedProduct.Id
                });
            }

            factor.Products = factorProducts;
            _factors.Add(factor);
        }

        public async Task<IList<ListFactorViewModel>> GetUserFactor(int userId)
        {
            return await _factors.Where(f => f.UserId == userId)
                .OrderByDescending(f => f.IssueDate)
                .Select(f => new ListFactorViewModel
                {
                    PhoneNumber = f.PhoneNumber,
                    LastName = f.LastName,
                    Address = f.Address,
                    Id = f.Id,
                    Name = f.Name,
                    IssueDate = f.IssueDate,
                    Status = f.Status,
                    Products = f.Products.Select(p=>new ListFactorProductViewModel
                    {
                        Id = p.Id,
                        Price = p.Price,
                        Count = p.Count,
                        ProductId = p.ProductId,
                        ProductName = p.Product.Title
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<DataGridViewModel<FactorDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form, DateTimeType dateTimeType,
            int page, int pageSize)
        {
            var usersQuery = _factors.AsQueryable();

            var query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(usersQuery);

            var dataGridModel = new DataGridViewModel<FactorDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<FactorDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };
            return dataGridModel;
        }

        public void Delete(int id)
        {
            _factors.Remove(_factors.Find(id));
        }

        public async Task<ListFactorViewModel> GetForEdit(int id)
        {
            return await _factors.Where(f => f.Id == id)
                .OrderByDescending(f => f.IssueDate)
                .Select(f => new ListFactorViewModel
                {
                    PhoneNumber = f.PhoneNumber,
                    LastName = f.LastName,
                    Address = f.Address,
                    Id = f.Id,
                    Name = f.Name,
                    IssueDate = f.IssueDate,
                    Status = f.Status,
                    Products = f.Products.Select(p => new ListFactorProductViewModel
                    {
                        Id = p.Id,
                        Price = p.Price,
                        Count = p.Count,
                        ProductId = p.ProductId,
                        ProductName = p.Product.Title,
                        MaxCount = p.Product.Count
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task Edit(ListFactorViewModel factorViewModel)
        {
            var selectedFactor = await _factors.Where(f => f.Id == factorViewModel.Id).Include(f => f.Products)
                .FirstOrDefaultAsync();

            selectedFactor.Status = factorViewModel.Status;
            selectedFactor.Address = factorViewModel.Address;
            selectedFactor.PhoneNumber = factorViewModel.PhoneNumber;
            selectedFactor.Name = factorViewModel.Name;
            selectedFactor.LastName = factorViewModel.LastName;

            foreach (var factorProduct in selectedFactor.Products.ToList())
            {
                var selectedProduct = factorViewModel.Products.FirstOrDefault(x => x.ProductId == factorProduct.ProductId);
                if (selectedProduct == null)
                {
                    _unitOfWork.MarkAsDeleted(factorProduct);
                    continue;
                }

                factorProduct.Count = selectedProduct.Count;
                _unitOfWork.MarkAsChanged(factorProduct);
            }

            _unitOfWork.MarkAsChanged(selectedFactor);
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
    }
}
