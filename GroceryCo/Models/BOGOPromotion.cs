using System;

namespace GroceryCo.Models
{
    /// <summary>
    /// Buy one get one free, or Buy one get one for 50%
    /// </summary>
    public class BOGOPromotion : Promotion
    {
        public int QualifyingQuantity { get; set; }

        public decimal AdditionalItemDiscountPercent { get; set; }

        public override string GetDescription()
        {
            return AdditionalItemDiscountPercent == 1.0m
                ? $"Buy {QualifyingQuantity} get one for free"
                : $"Buy {QualifyingQuantity} get one for {AdditionalItemDiscountPercent:p}";
        }

        /// <summary>
        /// Calculates effective price if quantity qualifies for this promotion
        /// </summary>
        /// <param name="quantity">Quantity purchased</param>
        /// <param name="regularPrice">Regular price of item</param>
        /// <returns>Effective price if qualified for promotion, regular price otherwise</returns>
        public override decimal GetEffectivePrice(int quantity, decimal regularPrice)
        {
            //1. Sanity checks
            if (QualifyingQuantity <= 0)
                throw new InvalidOperationException();

            if (quantity <= 0)
                return 0m;

            if (regularPrice <= 0m)
                return regularPrice;

            //2. quantity must be one more than qualifying quantity for this promotion to take effect
            if (quantity <= QualifyingQuantity)
                return regularPrice;

            //3. quantity is at least one more than level needed for this promotion
            var quotient = quantity / (QualifyingQuantity + 1);           

            var total = ((quantity - quotient) * regularPrice) + (quotient * (regularPrice * AdditionalItemDiscountPercent));
            var effectivePrice = total / quantity;
          
            return effectivePrice;
        }
    }
}
