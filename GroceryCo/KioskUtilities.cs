using GroceryCo.Models;
using System;
using System.Collections.Generic;

namespace GroceryCo
{
    public static class KioskUtilities
    {        
        /// <summary>
        /// Helper method for creating a sales transaction for given product ids.
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public static ISaleTransaction CreateSaleTransaction(ICollection<int> productIds)
        {
            if (productIds == null)
                throw new ArgumentNullException(nameof(productIds));

            var transaction = new SaleTransaction() { Id = Guid.NewGuid() };
            
            foreach(var productId in productIds)            
                transaction.AddItem(productId);            
                        
            return transaction;            
        }        
    }
}
