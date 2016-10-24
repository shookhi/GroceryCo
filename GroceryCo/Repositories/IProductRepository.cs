using System.Collections.Generic;
using GroceryCo.Models;

namespace GroceryCo.Repositories
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProducts(ICollection<int> ids);
    }
}
