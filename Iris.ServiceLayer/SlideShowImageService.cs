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
    public class SlideShowImageService : ISlideShowImageService
    {
        #region Fields
        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<SlideShowImage> _slideShowImages;
        #endregion

        #region Constractors
        public SlideShowImageService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _slideShowImages = unitOfWork.Set<SlideShowImage>();
            _mappingEngine = mappingEngine;
        }
        #endregion

        #region GetDataGridSource
        public async Task<DataGridViewModel<SlideShowDataGridViewModel>> GetDataGridSource(string orderBy, JqGridRequest request, NameValueCollection form, DateTimeType dateTimeType,
            int page, int pageSize)
        {
            var usersQuery = _slideShowImages.AsQueryable();

            var query = new JqGridSearch(request, form, dateTimeType).ApplyFilter(usersQuery);

            var dataGridModel = new DataGridViewModel<SlideShowDataGridViewModel>
            {
                Records = await query.AsQueryable().OrderBy(orderBy)
                    .Skip(page * pageSize)
                    .Take(pageSize).ProjectTo<SlideShowDataGridViewModel>(null, _mappingEngine).ToListAsync(),

                TotalCount = await query.AsQueryable().OrderBy(orderBy).CountAsync()
            };

            return dataGridModel;
        }
        #endregion

        #region AddSlide
        public void AddSlide(SlideShowImage slideShow, IList<SlideShowImage> otherSlideShows)
        {

            _slideShowImages.Add(slideShow);
            FixOrder(otherSlideShows);
        }
        #endregion

        #region FixOrder
        private void FixOrder(IList<SlideShowImage> otherSlideShows)
        {
            foreach (var slideShow in otherSlideShows)
            {
                _slideShowImages.Attach(slideShow);
                _unitOfWork.Entry(slideShow).Property(slide => slide.Order).IsModified = true;
            }
        }
        #endregion

        #region DeleteSlide
        public void DeleteSlide(int slideId)
        {
            var entity = new SlideShowImage() { Id = slideId };

            _unitOfWork.Entry(entity).State = EntityState.Deleted;
        }
        #endregion

        #region EditSlide
        public void EditSlide(SlideShowImage slideShow, IList<SlideShowImage> otherSlideShows)
        {
            _slideShowImages.Attach(slideShow);
            _unitOfWork.Entry(slideShow).State = EntityState.Modified;


            FixOrder(otherSlideShows);
        }
        #endregion

        #region GetSlideShowImages
        public async Task<IList<SlideShowViewModel>> GetSlideShowImages()
        {
            return await _slideShowImages
                        .AsNoTracking()
                        .OrderBy(slide => slide.Order).ThenByDescending(slide => slide.CreatedDate)
                        .ProjectTo<SlideShowViewModel>()
                        .Cacheable().ToListAsync();
        }
        #endregion

        #region GetSlideShow
        public async Task<SlideShowViewModel> GetSlideShow(int slideShowId)
        {
            return await _slideShowImages.Where(slideShow => slideShow.Id == slideShowId)
                .ProjectTo<SlideShowViewModel>().FirstOrDefaultAsync();
        }
        #endregion
    }
}
