using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThreadedIRCBot
{   
    [TestClass]
    class Define_Tests
    {
        [TestMethod]
        public void defineCat()
        {
            Assert.AreEqual("fwef", Define.DefineWord("cat"));
        }
    }
}
