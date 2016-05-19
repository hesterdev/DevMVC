using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace PrimeNum.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_PrimeNum_IsMath()
        {
            // arrange
            PrmNum target = new PrmNum();

            // set 
            bool ret1 = target.IsMath(1);
            bool ret2 = target.IsMath(2);
            bool ret3 = target.IsMath(3);
            bool ret4 = target.IsMath(4);

            bool ret12 = target.IsMath(12);

            bool ret73 = target.IsMath(73);

            // assert
            Assert.AreEqual(false, ret1);
            Assert.AreEqual(true, ret2);
            Assert.AreEqual(true, ret3);
            Assert.AreEqual(false, ret4);
            Assert.AreEqual(false, ret12);
            Assert.AreEqual(true, ret73);
        }
    }
}
