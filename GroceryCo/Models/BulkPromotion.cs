using System;
using System.Globalization;

namespace GroceryCo.Models
{
    /// <summary>
    /// Bulk promotion types, example 'Buy 3 for $5'
    /// </summary>
    public class BulkPromotion : Promotion
    {
        public int QualifyingQuantity { get; set; }

        public decimal BulkPrice { get; set; }

        public override string GetDescription()
        {
           return string.Format("Get {0} for {1}", QualifyingQuantity,
                BulkPrice.ToString("C2", CultureInfo.CreateSpecificCulture("en-CA")));
        }

        /// <summary>
        /// Calculates effective price if quantity qualifies for this promotion
        /// </summary>
        /// <param name="quantity">Quantity purchased</param>
        /// <param name="regularPrice">Regular price of item</param>
        /// <returns>Effective price if qualified for promotion, regular price otherwise</returns>
        public override decimal GetEffectivePrice(int quantity, decimal regularPrice)
        {
            //1. sanity checks
            if (QualifyingQuantity <= 0)
                throw new InvalidOperationException();

            if (quantity <= 0)
                return 0m;

            if (regularPrice <= 0m)
                return regularPrice;

            //2. quantity hasn't reached level needed to qualify for this promotion
            if (quantity < QualifyingQuantity)
                return regularPrice;            
                        
            //3. quantity is equal to or more than level needed for this promotion
            var quotient = quantity / QualifyingQuantity;
            var remainder = quantity % QualifyingQuantity;

            var total = (quotient * BulkPrice) + (remainder * regularPrice);
            var effectivePrice = total / quantity;

            return effectivePrice;
        }
    }
}
