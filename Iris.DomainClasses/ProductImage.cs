namespace Iris.DomainClasses
{
    public class ProductImage : BaseEntity
    {
        public string Name { get; set; }
        public int? Order { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Url { get; set; }
        public int Size { get; set; }
        public string DeleteUrl { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
