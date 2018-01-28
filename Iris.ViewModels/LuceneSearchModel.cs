﻿namespace Iris.ViewModels
{
    #region LuceneSearchModel
    public class LuceneSearchModel
    {
        #region Properties
        public int? ProductId { get; set; }
        public int? PostId { get; set; }
        public string Title { get; set; }
        public string SlugUrl { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string ProductStatus { get; set; }
        public string Price { get; set; }
        public string Discount { get; set; }
        #endregion
    }
    #endregion
}
