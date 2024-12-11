using Ecommerce;

namespace ProductService.Models 
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public Google.Type.Money Price { get; set; }
        public int Stock { get; set; }
    }
}
