using System;
using System.Collections.Generic;
using ThreadedIRCBot;

namespace TestURLModule
{
    using NUnit.Framework;

    [TestFixture]
    public class FOAASTests
    {
        [Test]
        public void GetResponse()
        {
            Assert.AreEqual(FOAAS.GetResponse("nugget/graymalkin/bunu"), "Well graymalkin, aren't you a shining example of a rancid fuck-nugget. - bunu");
            Assert.AreEqual(FOAAS.GetResponse("you/graymalkin"), "Fuck you. - graymalkin");
        }
    }
}
