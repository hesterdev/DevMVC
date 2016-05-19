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



            var discounter = new MinumumDiscountHelper();
            var target = new LinqValueCalculator(discounter);
            //var goalTotal = discounter.ApplyDiscount(products.Sum(e => e.Price));
            var goalTotal = products.Sum(e => e.Price);

            // act
            var result = target.ValueProducts(products);

            // assert 
            Assert.AreEqual(goalTotal, result);
        }
    }
}
