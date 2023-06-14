using WebShop.Models.ShopEntities;

namespace WebShop.Models.Mappings
{
    public class ProductsCategoriesMapping
    {

        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }


    }
}
