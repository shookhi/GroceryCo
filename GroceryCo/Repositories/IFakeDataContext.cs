using GroceryCo.Models;
using System.Collections.Generic;

namespace GroceryCo.Repositories
{
    public interface IFakeDataContext
    {       
        IEnumerable<Product> Products { get; set; }
        IEnumerable<Promotion> Promotions { get; set; }
        void InitWithSampleData();
    }
}
