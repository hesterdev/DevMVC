using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using EssentialTools.Models;
using Moq;

namespace EssentialTools.Tests
{
    [TestClass]
    public class UnitTest2
    {
        private Product[] products = {
            new Product {Name="Kayak",Price=275M },
            new Product {Name="Lifejacket",Price=48.95M },
            new Product {Name="Soccer ball",Price=19.50M },
            new Product {Name="Corner flag",Price=34.95M }
        };

        [TestMethod]
        public void Sum_Products_Correctly()
        {
            // arrange 
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
                .Returns<decimal>(total => total);
            var target = new LinqValueCalculator(mock.Object);

            // act
            var result = target.ValueProducts(products);

            // assert
            Assert.AreEqual(products.Sum(e => e.Price), result);

            /*
            var discounter = new MinumumDiscountHelper();
            var target = new LinqValueCalculator(discounter);
            //var goalTotal = discounter.ApplyDiscount(products.Sum(e => e.Price));
            var goalTotal = products.Sum(e => e.Price);

            // act
            var result = target.ValueProducts(products);

            // assert 
            Assert.AreEqual(goalTotal, result);
            */
        }

        private Product[] CreateProduct(decimal value)
        {
            return new[] { new Product { Price = value } };
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void Pass_Through_Variable_Discounts()
        {
            // arrange
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>())).Returns<decimal>(total => total);
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v == 0))).Throws<ArgumentOutOfRangeException>();
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v > 100))).Returns<decimal>(total => (total * 0.9M));
            mock.Setup(m => m.ApplyDiscount(It.IsInRange<decimal>(10, 100, Range.Inclusive))).Returns<decimal>(total => total - 5);
            var target = new LinqValueCalculator(mock.Object);

            // act
            decimal FiveDollarDiscount = target.ValueProducts(CreateProduct(5));
            decimal TenDollarDiscount = target.ValueProducts(CreateProduct(10));
            decimal FiftyDollarDiscount = target.ValueProducts(CreateProduct(50));
            decimal HundredDollarDiscount = target.ValueProducts(CreateProduct(100));
            decimal FiveHundredDollarDiscount = target.ValueProducts(CreateProduct(500));

            // assert
            Assert.AreEqual(5, FiveDollarDiscount, "$5 fail");
            Assert.AreEqual(5, TenDollarDiscount, "$10 fail");
            Assert.AreEqual(45, FiftyDollarDiscount, "$50 fail");
            Assert.AreEqual(95, HundredDollarDiscount, "$100 fail");
            Assert.AreEqual(450, FiveHundredDollarDiscount, "$500 fail");
            target.ValueProducts(CreateProduct(0));


        }
    }
}
