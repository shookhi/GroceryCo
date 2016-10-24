using GroceryCo.Models;
using System.Collections.Generic;
using System.Linq;

namespace GroceryCo.Repositories
{
    /// <summary>
    /// Repository for accessing products catalog
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private IFakeDataContext _context;

        public ProductRepository(IFakeDataContext context)
        {
            _context = context;
        }

        public Product GetProduct(int id)
        {
            return _context.Products.SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }

        public IEnumerable<Product> GetProducts(ICollection<int> ids)
        {
            if (!ids.Any())
                return Enumerable.Empty<Product>();

            return _context.Products.Where(p => ids.Contains(p.Id));
        }
    }
}
