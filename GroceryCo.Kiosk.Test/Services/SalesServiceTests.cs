using FluentAssertions;
using GroceryCo.Models;
using GroceryCo.Repositories;
using GroceryCo.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace GroceryCo.Kiosk.Test.Models
{
    [TestClass]
    public class SalesServiceTests
    {
        private IUnitOfWork _unitOfWork;
        private ISalesService _service;


        [TestInitialize]
        public void Setup()
        {
            //Use Moq here when not using IFakeDataContext
            _unitOfWork = new UnitOfWork();
            _service = new SalesService(_unitOfWork);        
        }

        [TestMethod]
        public void Checkout_WhenHasAnItem_SetsRegularPrice()
        {
            //Arrange                       
            var product = _unitOfWork.ProductRepository.GetProduct(1);
            var transaction = KioskUtilities.CreateSaleTransaction(new[] { product.Id });

            //Act
            _service.Checkout(transaction);

            //Assert
            transaction.GetItems().FirstOrDefault(i => i.ProductId == product.Id).RegularPrice
                .Should().Be(product.Price, "Current price of product should be set from products catalog");
        }

        [TestMethod]
        public void Checkout_WhenHasNoItem_HandlesGracefully()
        {
            //Arrange                                   
            var transaction = new SaleTransaction();

            //Act
            Action act = () => _service.Checkout(transaction);

            //Assert
            act.ShouldNotThrow<Exception>();
        }

        [TestMethod]
        public void Checkout_WhenHasAnItemWithNoCorrespondingProduct_ThrowsException()
        {
            //Arrange                                   
            var transaction = KioskUtilities.CreateSaleTransaction(new[] { 999 });

            //Act
            Action act = () => _service.Checkout(transaction);

            //Assert            
            act.ShouldThrow<Exception>().WithMessage($"Invalid product, id={999}");
        }

        [TestMethod]
        public void Checkout_WhenHasPercentDiscountPromotion_ShouldUseEffectivePrice()
        {
            //Arrange                       
            //A transaction with a product (id=4) which has a percent discount promotion (50% sale)            
            var transaction = KioskUtilities.CreateSaleTransaction(new[] { 4 });

            //Act
            _service.Checkout(transaction);

            //Assert
            var product = _unitOfWork.ProductRepository.GetProduct(4);
            var effectivePrice = product.Price * 0.50m;
            var effectiveTotal = 1 * effectivePrice;
            var regularTotal = 1 * product.Price;

            transaction.GetItems().FirstOrDefault().EffectivePrice.Should().Be(effectivePrice, "Product has 50% sale/discount");
            transaction.GetEffectiveTotal().Should().Be(effectiveTotal, "Total due should consider the promotion");
            transaction.GetRegularTotal().Should().Be(regularTotal, "Regular total should consider product's regular price");            
        }

        [TestMethod]
        public void Checkout_WhenHasProductWithPercentDiscountPromotion_AndRegularProducts_ShouldUseCorrectCorrespondingPrices()
        {
            //Arrange                       
            //sample 1: a sale with a product (id=4) which has a percent discount promotion (50% sale)            
            var transaction = KioskUtilities.CreateSaleTransaction(new[] { 4, 1, 1, 4, 1 });

            //Act
            _service.Checkout(transaction);

            //Assert
            var p1 = _unitOfWork.ProductRepository.GetProduct(1);
            var p4 = _unitOfWork.ProductRepository.GetProduct(4);

            var p4_effectivePrice = p4.Price * 0.50m;

            var regularTotal = (3 * p1.Price) + (2 * p4.Price); ;
            var effectiveTotal = (3 * p1.Price) + (2 * p4_effectivePrice);            

            transaction.GetItems().FirstOrDefault(i => i.ProductId == p1.Id)
                .EffectivePrice.Should().Be(p1.Price, "Product 1 has no promotion, so effective price is the same as regular price");

            transaction.GetItems().FirstOrDefault(i => i.ProductId == p4.Id)
                .EffectivePrice.Should().Be(p4_effectivePrice, "Product 4 has 50% sale promotion");

            transaction.GetRegularTotal().Should().Be(regularTotal, "Regular total should consider all products' regular price");
            transaction.GetEffectiveTotal().Should().Be(effectiveTotal, "Effective total should consider the promotions");            
        }

        [TestMethod]
        public void Checkout_WhenHasBulkPromotion_ShouldUseEffectivePrice()
        {
            //Arrange                       
            //A transaction with a product (id=6) which has a bulk promotion (Buy 3 for $5)        
            var transaction = KioskUtilities.CreateSaleTransaction(new[] { 6, 6, 6 });

            //Act
            _service.Checkout(transaction);

            //Assert
            var product = _unitOfWork.ProductRepository.GetProduct(6);
            var effectivePrice = 5.0m / 3;
            var effectiveTotal = 3 * effectivePrice;
            var regularTotal = 3 * product.Price;

            transaction.GetItems().FirstOrDefault().EffectivePrice.Should().Be(effectivePrice, "Product has a promotion of buy 3 for $5");
            transaction.GetEffectiveTotal().Should().Be(effectiveTotal, "Effective total should consider the promotion");
            transaction.GetRegularTotal().Should().Be(regularTotal, "Regular total should consider product's regular price");
        }

        [TestMethod]
        public void Checkout_WhenHasBulkPromotion_ButNotEnoughQuantity_ShouldUseRegularPrice()
        {
            //Arrange                       
            //A transaction with a product (id=6) which has a bulk promotion (Buy 3 for $5)        
            var transaction = KioskUtilities.CreateSaleTransaction(new[] { 6, 6 });

            //Act
            _service.Checkout(transaction);

            //Assert
            var product = _unitOfWork.ProductRepository.GetProduct(6);            
            var regularTotal = 2 * product.Price;

            transaction.GetItems().FirstOrDefault().EffectivePrice.Should().Be(product.Price, "Product has a promotion, but quantity is not enough");
            transaction.GetEffectiveTotal().Should().Be(regularTotal, "Effective total should consider regular price since quantity is not enough");
            transaction.GetRegularTotal().Should().Be(regularTotal, "Regular total should consider product's regular price");
        }

        [TestMethod]
        public void Checkout_WhenBOGOPromotion_ShouldUseEffectivePrice()
        {
            //Arrange                       
            //A transaction with a product (id=7) which has a bogo promotion (Buy 2, get one for 50%)     
            var transaction = KioskUtilities.CreateSaleTransaction(new[] { 7, 7, 7 });

            //Act
            _service.Checkout(transaction);

            //Assert
            var product = _unitOfWork.ProductRepository.GetProduct(7);            
            var effectiveTotal = (2 * product.Price) + (1 * (0.50m * product.Price));
            var effectivePrice = effectiveTotal / 3;
            var regularTotal = 3 * product.Price;

            transaction.GetItems().FirstOrDefault().EffectivePrice.Should().Be(effectivePrice, "Product has a promotion of buy 2 get one for 50%");
            transaction.GetEffectiveTotal().Should().Be(effectiveTotal, "Effective total should consider the promotion");
            transaction.GetRegularTotal().Should().Be(regularTotal, "Regular total should consider product's regular price");
        }

        [TestMethod]
        public void Checkout_WhenBOGOPromotion_ButNotEnoughQuantity_ShouldUseRegularPrice()
        {
            //Arrange                       
            //A transaction with a product (id=7) which has a bogo promotion (Buy 2, get one for 50%)     
            var transaction = KioskUtilities.CreateSaleTransaction(new[] { 7, 7 });

            //Act
            _service.Checkout(transaction);

            //Assert
            var product = _unitOfWork.ProductRepository.GetProduct(7);
            var regularTotal = 2 * product.Price;

            transaction.GetItems().FirstOrDefault().EffectivePrice.Should().Be(product.Price, "Product has a promotion, but quantity is not enough to qualify");
            transaction.GetEffectiveTotal().Should().Be(regularTotal, "Effective total should equal regular total, because quantity is not enough");
            transaction.GetRegularTotal().Should().Be(regularTotal, "Regular total should always consider product's regular price");
        }

        [TestMethod]
        public void Checkout_WhenProductHasMoreThanOnePromotion_ShouldGiveCustomerBestDeal()
        {
            //Arrange                       
            //A transaction with a product (id=8) which has two promotions (10% Student Discount, and Buy 3 for $5) 
            var transaction = KioskUtilities.CreateSaleTransaction(new[] { 8, 8, 8 });

            //Act
            _service.Checkout(transaction);

            //Assert
            //"10% Discount" is the best deal in this case (with consideration of regular price)
            var product = _unitOfWork.ProductRepository.GetProduct(8);
            var effectivePrice = (0.9m * product.Price);
            var effectiveTotal = 3 * effectivePrice;
            var regularTotal = 3 * product.Price;

            transaction.GetItems().FirstOrDefault().EffectivePrice.Should().Be(effectivePrice, "Product's best deal is 10% discount");
            transaction.GetEffectiveTotal().Should().Be(effectiveTotal, "Effective total should consider the promotion");
            transaction.GetRegularTotal().Should().Be(regularTotal, "Regular total should consider product's regular price");
        }
    }
}
