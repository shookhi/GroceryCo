using GroceryCo.Models;
using GroceryCo.Repositories;
using System;
using System.Linq;

namespace GroceryCo.Services
{
    /// <summary>
    /// Service for kiosk sales processes.
    /// </summary>
    public class SalesService : ISalesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Performs checkout process, using the latest product prices and promotions.
        /// If a product has multiple promotions, this method gives the customer the best deal
        /// </summary>
        /// <param name="transaction">Sales transaction to be processed</param>
        public void Checkout(ISaleTransaction transaction)
        {
            try
            {
                if (transaction == null)
                    throw new ArgumentNullException(nameof(transaction));                

                if (!transaction.GetItems().Any())
                    return;

                var products = _unitOfWork.ProductRepository.GetProducts(transaction.GetItems().Select(i => i.ProductId).ToList());

                foreach(var item in transaction.GetItems())
                {
                    var product = products.SingleOrDefault(p => p.Id == item.ProductId);
                    if (product == null)
                        throw new Exception($"Invalid product, id={item.ProductId}");

                    //set the latest regular price
                    item.RegularPrice = product.Price;

                    //find promotion to be applied, and effective price                    
                    item.EffectivePrice = product.Price;
                    foreach(var promotion in product.Promotions)
                    {
                        // -- Rule: Customer gets the best deal (least price)
                        var effectivePrice = promotion.GetEffectivePrice(item.Quantity, item.RegularPrice);
                        if(effectivePrice < item.EffectivePrice)
                        {
                            item.EffectivePrice = effectivePrice;
                            item.AppliedPromotionId = promotion.Id;                            
                        }
                    }                                           
                }
            }
            catch(Exception)
            {
                //service error handling goes here
                throw;
            }
        }                
    }
}
