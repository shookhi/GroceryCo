using GroceryCo.Models;

namespace GroceryCo.Services
{
    /// <summary>
    /// Interface for sales services
    /// </summary>
    public interface ISalesService
    {
        void Checkout(ISaleTransaction transaction);        
    }
}
