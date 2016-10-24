using FluentAssertions;
using GroceryCo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryCo.Kiosk.Test.Models
{
    [TestClass]
    public class BulkPromotionTests
    {
        private BulkPromotion _promotion;

        [TestInitialize]
        public void Setup()
        {
            _promotion = new BulkPromotion() { QualifyingQuantity = 3, BulkPrice = 5.0m };
        }

        [TestMethod]
        public void EffectivePrice_WhenQuantityLTQualifyingQuantity_ShouldUseRegularPrice()
        {
            //Arrange            
            var quantity = 2;
            var regularPrice = 2.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = regularPrice;
            result.Should().Be(expected, "Quantity hasn't reached level needed to qualify for this promotion");
        }

        [TestMethod]
        public void EffectivePrice_WhenQuantityEqualsQualifyingQuantity_ShouldCalculateEffectivePrice()
        {
            //Arrange            
            var quantity = _promotion.QualifyingQuantity;
            var regularPrice = 2.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = 5.0m / 3;
            result.Should().Be(expected, "Quantity purchases qualifies for this promotion.");
        }

        [TestMethod]
        public void EffectivePrice_WhenQuantityGTQualifyingQuantity_ShouldCalculateEffectivePrice()
        {
            //Arrange            
            var quantity = _promotion.QualifyingQuantity + 1;
            var regularPrice = 2.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = ((1 * _promotion.BulkPrice) + (1 * regularPrice)) / quantity;
            result.Should().Be(expected, "Quantity purchases qualifies for this promotion.");
        }

        [TestMethod]
        public void EffectivePrice_WhenQuantityTwiceQualifyingQuantity_ShouldCalculateEffectivePrice()
        {
            //Arrange            
            var quantity = _promotion.QualifyingQuantity * 2;
            var regularPrice = 2.0m;

            //Act
            var result = _promotion.GetEffectivePrice(quantity, regularPrice);

            //Assert
            var expected = (2 * _promotion.BulkPrice) / quantity;
            result.Should().Be(expected, "Quantity purchased qualifies for this promotion.");
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