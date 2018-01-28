using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EFSecondLevelCache;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;

namespace Iris.ServiceLayer
{
    public class SiteMapService : ISiteMapService
    {
        #region Fields
        private readonly IDbSet<Product> _products;
        #endregion

        #region Constractors
        public SiteMapService(IUnitOfWork unitOfWork)
        {
            _products = unitOfWork.Set<Product>();
        }
        #endregion

        #region GetProductsSiteMap
        public async Task<IList<SiteMapLinkViewModel>> GetProductsSiteMap()
        {
            return await _products.OrderByDescending(p => p.PostedDate)
                .Select(p => new SiteMapLinkViewModel
                {
                    Id = p.Id,
                    SlugUrl = p.SlugUrl,
                    LastModified = p.PostedDate
                }).Cacheable().ToListAsync();
        }
        #endregion
    }
}
