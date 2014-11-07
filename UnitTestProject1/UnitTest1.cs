using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadedIRCBot;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class URLTests
    {
        [TestMethod]
        public void GetTitle()
        {
            Assert.AreEqual(ThreadedIRCBot.URL.getTitle(@"http://www.google.com/"), "Google");
        }

        [TestMethod]
        public void GetURL()
        {
            string[] k = {"http://www.flickr.com/photos/phaseviii/9217207106/in/set-72157626056708495"};
            List<String> m = ThreadedIRCBot.URL.getURLs(k);

            Assert.AreEqual(k[0], m[0]);
        }
    }
}
