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
        [TestCategory("Helper")]
        public void BytesToSize()
        {
            Assert.AreEqual(URL.size_to_string((long)(1024 * 1024 * 1024 * 1.2)), "1.2 GB");
        }

        [TestMethod]
        public void GetTitle()
        {
            Assert.AreEqual(URL.getTitle(@"http://www.google.com/"), "Google");
            Assert.AreEqual(URL.getTitle(@"http://www.bbc.co.uk/news/uk-politics-29956289"), "BBC News - Osborne's EU budget claim challenged");
            Assert.AreEqual(URL.getTitle(@"https://www.youtube.com/watch?v=zqKZ_WIK5ms"), "Yaël Naïm - Toxic - YouTube");
            Assert.AreEqual(URL.getTitle(@"http://i.imgur.com/VmRmHMJ.jpg"), "[image/jpeg 58.3 KB]");
            Assert.AreEqual(URL.getTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-DVD.iso"), "[application/x-iso9660-image 3.9 GB]");
            Assert.AreEqual(URL.getTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-KdeLive.iso"), "[application/x-iso9660-image 1.2 GB]");
            Assert.AreEqual(URL.getTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-Minimal.iso"), "[application/x-iso9660-image 566 MB]");
        }

        [TestMethod]
        public void GetTitleUASpoof()
        {
            Assert.AreEqual(URL.getTitle(@"https://www.facebook.com/gray.malkin1"), "Simon Moore | Facebook");
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
