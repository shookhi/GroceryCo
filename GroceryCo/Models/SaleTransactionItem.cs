namespace GroceryCo.Models
{
    /// <summary>
    /// Model for individual sale transaction items
    /// </summary>
    public class SaleTransactionItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        
        //data set at checkout
        public int AppliedPromotionId { get; set; }

        public decimal RegularPrice { get; set; }
        
        public decimal EffectivePrice { get; set; }               
    }
}