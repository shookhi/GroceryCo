namespace GroceryCo.Models
{
    /// <summary>
    /// Percent discount promotions, example: 50% sale, 10% discount
    /// </summary>
    public class PercentDiscountPromotion : Promotion
    {
        public decimal DiscountPercent { get; set; }

        public override string GetDescription()
        {
            return $"{DiscountPercent:p} Sale";
        }

        /// <summary>
        /// Calculates effective price if quantity qualifies for this promotion
        /// </summary>
        /// <param name="quantity">Quantity purchased</param>
        /// <param name="regularPrice">Regular price of item</param>
        /// <returns>Discounted price</returns>
        public override decimal GetEffectivePrice(int quantity, decimal regularPrice)
        {
            var effectivePrice = (1.0m - DiscountPercent) * regularPrice;
            return effectivePrice;
        }
    }
}
