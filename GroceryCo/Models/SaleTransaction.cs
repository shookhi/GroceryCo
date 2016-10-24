using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceryCo.Models
{
    /// <summary>
    /// Model for kiost sale transaction
    /// </summary>
    public class SaleTransaction : ISaleTransaction
    {
        private readonly ICollection<SaleTransactionItem> _items;

        public SaleTransaction()
        {
            _items = new HashSet<SaleTransactionItem>();
        }

        public Guid Id { get; set; }                               

        /// <summary>
        /// Getter of transaction items
        /// </summary>
        /// <returns></returns>
        public ICollection<SaleTransactionItem> GetItems()
        {
            return _items;
        }

        /// <summary>
        /// Adds new item for given product, or increment it's quantity if already existing
        /// </summary>
        /// <param name="productId"></param>
        public void AddItem(int productId)
        {
            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if(existingItem != null)
            {
                existingItem.Quantity++;                
            }
            else
            {
                var newItem = new SaleTransactionItem
                {
                    ProductId = productId,
                    Quantity = 1
                };

                _items.Add(newItem);
            }
        }

        /// <summary>
        /// Removes item for given product, or decrement it's quantity
        /// </summary>
        /// <param name="productId"></param>
        public void RemoveItem(int productId)
        {
            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem == null)
                return;                
            
            if(existingItem.Quantity == 1)            
                _items.Remove(existingItem);            
            else            
                existingItem.Quantity--;                                                 
        }       

        /// <summary>
        /// Clears items collections
        /// </summary>
        public void ClearItems()
        {
            _items.Clear();
        }        

        /// <summary>
        /// Uses effective price of the items to calculate transaction's total
        /// </summary>
        /// <returns></returns>
        public decimal GetEffectiveTotal()
        {
            var total = _items.Sum(i => i.Quantity * i.EffectivePrice);
            return total;
        }

        /// <summary>
        /// Uses regular price of the items to calculate transaction's total
        /// </summary>
        /// <returns></returns>
        public decimal GetRegularTotal()
        {
            var total = _items.Sum(i => i.Quantity * i.RegularPrice);
            return total;
        }
    }
}
