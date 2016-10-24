using GroceryCo.Models;
using System.Collections.Generic;
using System.Linq;

namespace GroceryCo.Repositories
{
    /// <summary>
    /// A fake data context for this exercise
    /// </summary>
    public class FakeDataContext : IFakeDataContext
    {
        public FakeDataContext()
        {
            InitWithSampleData();                     
        }

        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Promotion> Promotions { get; set; }

        public void InitWithSampleData()
        {
            Promotions = new HashSet<Promotion>()
            {
                new PercentDiscountPromotion { Id = 1, Name = "Winter 2016 Pudding Sale", DiscountPercent = 0.50m },
                new PercentDiscountPromotion { Id = 2, Name = "Student Discount", DiscountPercent = 0.10m },

                new BulkPromotion { Id = 3, Name = "2016 Bulk Apple Promotion", QualifyingQuantity = 3, BulkPrice = 5.0m },

                new BOGOPromotion { Id = 4, Name = "Buy 2, get one for 50%", QualifyingQuantity = 2, AdditionalItemDiscountPercent = 0.5m }
            };

            Products = new HashSet<Product>()
            {
                //products with no promotion
                new Product { Id = 1, Name = "Orange", Price = 2.33m, Promotions = new List<Promotion>() },
                new Product { Id = 2, Name = "Banana", Price = 1.0m, Promotions = new List<Promotion>() },
                new Product { Id = 3, Name = "Pear", Price = 5.8m, Promotions = new List<Promotion>() },

                //products with 1 promotion
                new Product { Id = 4, Name = "Pudding", Price = 10.99m, Promotions = Promotions.Where(p => p.Id == 1).ToList() },
                new Product { Id = 5, Name = "Frozen Pizza", Price = 6.76m, Promotions = Promotions.Where(p => p.Id == 2).ToList() },
                new Product { Id = 6, Name = "Apple", Price = 2.0m, Promotions = Promotions.Where(p => p.Id == 3).ToList() },
                new Product { Id = 7, Name = "Fresh Bagel", Price = 3.5m, Promotions = Promotions.Where(p => p.Id == 4).ToList() },

                //products with multiple promotions
                new Product { Id = 8, Name = "Red Apple", Price = 1.5m,
                    Promotions = Promotions.Where(p => new [] { 2, 3 }.Contains(p.Id)).ToList() },
                new Product { Id = 9, Name = "Chocolate Pudding", Price = 13.5m,
                    Promotions = Promotions.Where(p => new [] { 1, 2, 4 }.Contains(p.Id)).ToList() },
            };
        }
    }
}
