using FluentAssertions;
using GroceryCo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryCo.Kiosk.Test.Models
{
    [TestClass]
    public class PercentDiscountPromotionTests
    {                
        [TestMethod]
        public void EffectivePrice_ShouldApplyPercentDiscount()
        {
            //Arrange
            var promotion = new PercentDiscountPromotion() { DiscountPercent = 0.1m };
            var quantity = 0;
            var regularPrice = 45.0m;

            //Act
            var result = promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = 0.9m * 45.0m;
            result.Should().Be(expected, "10% off from $45 is $40.5");
        }
    }
}
