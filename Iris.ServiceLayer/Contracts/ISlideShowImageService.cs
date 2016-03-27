using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Iris.DomainClasses;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.ServiceLayer.Contracts
{
    public interface ISlideShowImageService
    {
        Task<DataGridViewModel<SlideShowDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request,
            NameValueCollection form, DateTimeType dateTimeType, int page, int pageSize);

        void AddSlide(SlideShowImage slideShow,IList<SlideShowImage> otherSlideShows);
        void DeleteSlide(int slideId);
        void EditSlide(SlideShowImage slideShow, IList<SlideShowImage> otherSlideShows);
        Task<IList<SlideShowViewModel>> GetSlideShowImages();
        Task<SlideShowViewModel> GetSlideShow(int slideShowId);
    }
}
