using System;
using System.Collections.Generic;

namespace GroceryCo.Models
{
    public interface ISaleTransaction
    {
        Guid Id { get; set; }

        ICollection<SaleTransactionItem> GetItems();
        void AddItem(int productId);
        void RemoveItem(int productId);        
        void ClearItems();
        decimal GetEffectiveTotal();
        decimal GetRegularTotal();
    }
}
