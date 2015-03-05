using System;
using ThreadedIRCBot;
using System.Collections.Generic;

namespace UnitTestProject1
{
    using NUnit.Framework;

    [TestFixture]
    public class URLTests
    {
        [Test]
        public void BytesToSize()
        {
            Assert.AreEqual(URL.size_to_string((long)(1024 * 1024 * 1024 * 1.2)), "1.2 GiB");
        }

        [Test]
        public void GetTitle()
        {
            Assert.AreEqual(URL.getTitle(@"http://www.google.com/"), "Title: Google");
            Assert.AreEqual(URL.getTitle(@"http://www.bbc.co.uk/news/uk-politics-29956289"), "Title: BBC News - Osborne's EU budget claim challenged");
            Assert.AreEqual(URL.getTitle(@"https://www.youtube.com/watch?v=zqKZ_WIK5ms"), "Title: Yaël Naïm - Toxic - YouTube");
            Assert.AreEqual(URL.getTitle(@"http://imgur.com/gallery/G2YpErL"), "Title: Cows: shampooed, conditioned, and blow dried. - Imgur");
            Assert.AreEqual(URL.getTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-DVD.iso"), "[application/x-iso9660-image 3.9 GiB]");
            Assert.AreEqual(URL.getTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-KdeLive.iso"), "[application/x-iso9660-image 1.2 GiB]");
            Assert.AreEqual(URL.getTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-Minimal.iso"), "[application/x-iso9660-image 566 MiB]");
        }

        [Test]
        public void GetTitleUASpoof()
        {
            // Assert.AreEqual(URL.getTitle(@"https://www.facebook.com/gray.malkin1"), "Title: Simon Cooksey | Facebook");
        }

        [Test]
        public void GetURL()
        {
            string[] k = {"http://www.flickr.com/photos/phaseviii/9217207106/in/set-72157626056708495"};
            List<String> m = ThreadedIRCBot.URL.getURLs(k);

            Assert.AreEqual(k[0], m[0]);
        }
    }

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
