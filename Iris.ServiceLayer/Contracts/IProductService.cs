using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Iris.DomainClasses;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer.Contracts
{
    public interface IProductService
    {
        Task AddProduct(Product product);

        Task<IList<ProductImage>>  EditProduct(Product editedProduct);

        Task<AddProductViewModel> GetProductForEdit(int productId);

        Task<DataGridViewModel<ProductDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
            NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize);

        void DeleteProduct(int productId);
        Task<IList<ProductWidgetViewModel>> GetNewestProducts(int count);
        Task<IList<ProductWidgetViewModel>> GetMostViewedProducts(int count);
        Task<IList<ProductWidgetViewModel>> GetPopularProducts(int count);
        Task<IList<decimal>> GetAvailableProductPrices();
        Task<IList<ProductWidgetViewModel>> SearchProduct(SearchProductViewModel searchModel);
        Task<ProductPageViewModel> GetProductPage(int productId);
        Task UpdateViewNumber(int productId);
        Task SaveRating(int productId, double rating);
        Task<IList<LueneProduct>> GetAllForLuceneIndex();
        Task<IList<string>> GetProductImages(int productId);
    }

    public class LueneProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string SlugUrl { get; set; }
        public string Category { get; set; }
    }
}

