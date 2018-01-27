namespace Iris.ViewModels
{
    public class SearchProductViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public bool ShowStockProductsOnly { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinDiscount { get; set; }
        public decimal? MaxDiscount { get; set; }

        public int[] SelectedCategories { get; set; }
        public string SearchTerm { get; set; }

    }
}