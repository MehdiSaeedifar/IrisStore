﻿using System.Threading.Tasks;
using System.Web.Mvc;
using Iris.ServiceLayer.Contracts;
using Iris.Web.SiteMap;

namespace Iris.Web.Controllers
{
    #region SiteMapController
    [RoutePrefix("")]
    public partial class SiteMapController : Controller
    {
        #region Feilds
        private readonly ISiteMapService _siteMapService;
        #endregion

        #region Constractors
        public SiteMapController(ISiteMapService siteMapService)
        {
            _siteMapService = siteMapService;
        }
        #endregion

        #region Index
        [Route("~/sitemap.xml")]
        public virtual async Task<ActionResult> Index()
        {
            var productsList = await _siteMapService.GetProductsSiteMap();

            var sm = new Sitemap();

            foreach (var product in productsList)
            {
                sm.Add(new Location()
                {
                    Url = Url.Action(MVC.Product.Home.ActionNames.Index, MVC.Product.Home.Name, new { area = MVC.Product.Name, id = product.Id, title = product.SlugUrl }, Request.Url.Scheme),
                    LastModified = product.LastModified.Date,
                    Priority = 0.5D,
                    ChangeFrequency = Location.EChangeFrequency.Daily
                });
            }

            return new SiteMapResult(sm);
        }
        #endregion
    }
    #endregion
}