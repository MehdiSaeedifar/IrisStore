using System.Collections.Generic;
using System.Threading.Tasks;
using Iris.ViewModels;

namespace Iris.ServiceLayer.Contracts
{
    public interface ISiteMapService
    {
        Task<IList<SiteMapLinkViewModel>> GetProductsSiteMap();
    }
}
