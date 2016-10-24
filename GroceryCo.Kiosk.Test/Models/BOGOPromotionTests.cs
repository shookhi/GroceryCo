using FluentAssertions;
using GroceryCo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryCo.Kiosk.Test.Models
{
    [TestClass]
    public class BOGOPromotionTests
    {
        private BOGOPromotion _promotion;
      
        [TestInitialize]
        public void Setup()
        {
            //buy 2, get one for 50%
            _promotion = new BOGOPromotion() { QualifyingQuantity = 2, AdditionalItemDiscountPercent = 0.50m };
        }

        [TestMethod]
        public void EffectivePrice_WhenQuantityLTQualifyingQuantity_ShouldUseRegularPrice()
        {
            //Arrange            
            var quantity = 1;
            var regularPrice = 2.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = regularPrice;
            result.Should().Be(expected, "Quantity hasn't reached level needed to qualify for this promotion");
        }

        [TestMethod]
        public void EffectivePrice_WhenQuantityEqualsQualifyingQuantity_ShouldUseRegularPrice()
        {
            //Arrange            
            var quantity = _promotion.QualifyingQuantity;
            var regularPrice = 2.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = regularPrice;
            result.Should().Be(expected, "In order to qualify for this promotion, an additional item should be purchased.");
        }

        [TestMethod]
        public void EffectivePrice_WhenQuantityGTQualifyingQuantity_4_ShouldCalculateEffectivePrice()
        {
            //Arrange            
            var quantity = 4;
            var regularPrice = 2.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = ((3 * regularPrice) + (1 * 0.50m * regularPrice)) / quantity;
            result.Should().Be(expected, "One item qualifies for this discount.");
        }

        [TestMethod]
        public void EffectivePrice_WhenQuantityGTQualifyingQuantity_17_ShouldCalculateEffectivePrice()
        {
            //Arrange            
            var quantity = 17;
            var regularPrice = 2.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = ((12 * regularPrice) + (5 * 0.50m * regularPrice)) / quantity;
            result.Should().Be(expected, "Five items qualify for this discount.");
        }      

        [TestMethod]
        public void EffectivePrice_WhenRegularPriceIsZero_ShouldReturnZero()
        {
            //Arrange            
            var quantity = 2;
            var regularPrice = 0.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = 0.0m;
            result.Should().Be(expected, "Regular price is zero.");
        }

        [TestMethod]
        public void EffectivePrice_WhenQuantityIsZero_ShouldReturnZero()
        {
            //Arrange            
            var quantity = 0;
            var regularPrice = 2.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = 0.0m;
            result.Should().Be(expected, "Quantity is zero.");
        }
    }
}