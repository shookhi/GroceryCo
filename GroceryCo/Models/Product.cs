using System.Collections.Generic;

namespace GroceryCo.Models
{
    /// <summary>
    /// Model for products sold at GroceryCo
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
        
        public ICollection<Promotion> Promotions { get; set; }                
    }
}