using FluentAssertions;
using GroceryCo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GroceryCo.Kiosk.Test.Models
{
    [TestClass]
    public class SaleTransactionTests
    {
        private ISaleTransaction _transaction;

        [TestInitialize]
        public void Setup()
        {
            _transaction = new SaleTransaction();                        
        }

        [TestMethod]
        public void AddItem_NewProduct_ShouldAddItemEntry()
        {
            //Arrange            
            _transaction.AddItem(1);

            //Act
            _transaction.AddItem(2);

            //Assert
            _transaction.GetItems().Count.Should().Be(2, "Two different products added");
            _transaction.GetItems().FirstOrDefault().Quantity.Should().Be(1, "We added one item of the product 1");            
        }

        [TestMethod]
        public void AddItem_ExistingProduct_ShouldIncrementQuantity()
        {
            //Arrange            
            _transaction.AddItem(1);

            //Act
            _transaction.AddItem(1);

            //Assert
            _transaction.GetItems().Count.Should().Be(1, "Only product added");
            _transaction.GetItems().FirstOrDefault().Quantity.Should().Be(2, "We added two items of the same product");
        }

        [TestMethod]
        public void RemoveItem_ExistingProductWithQtyOne_ShouldRemoveItemEntry()
        {
            //Arrange            
            _transaction.AddItem(1);

            //Act
            _transaction.RemoveItem(1);

            //Assert
            _transaction.GetItems().Count.Should().Be(0, "We removed the only existing item");            
        }

        [TestMethod]
        public void AddItem_ExistingProductWithQtyTwo_ShouldDecrementQuantity()
        {
            //Arrange            
            _transaction.AddItem(1);
            _transaction.AddItem(1);

            //Act
            _transaction.RemoveItem(1);

            //Assert
            _transaction.GetItems().Count.Should().Be(1, "Two items of the product existed");
            _transaction.GetItems().FirstOrDefault().Quantity.Should().Be(1, "We removed one of the items");
        }

        [TestMethod]
        public void RemoveItem_NonExistingProduct_ShouldDoNothing()
        {
            //Arrange            
            _transaction.AddItem(1);

            //Act
            _transaction.RemoveItem(2);

            //Assert
            _transaction.GetItems().Count.Should().Be(1, "We try removing non-existing item");
            _transaction.GetItems().FirstOrDefault().ProductId.Should().Be(1, "Pre-existing product");
        }
    }
}
