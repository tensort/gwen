using System;
using ThreadedIRCBot;
using System.Collections.Generic;

namespace TestURLModule
{
    using NUnit.Framework;

    [TestFixture]
    public class URLTests
    {
        [Test]
        public void BytesToSize()
        {
            Assert.AreEqual(URL.SizeToString((long)(1024 * 1024 * 1024 * 1.2)), "1.2 GiB");
        }

        [Test]
        public void GetTitleSimple()
        {
            Assert.AreEqual(URL.GetTitle(@"http://www.google.com/", true), "Title: Google");
            Assert.AreEqual(URL.GetTitle(@"http://www.bbc.co.uk/news/uk-politics-29956289", true), "Title: BBC News - Osborne's EU budget claim challenged");
            Assert.AreEqual(URL.GetTitle(@"https://www.youtube.com/watch?v=zqKZ_WIK5ms", true), "Title: Yaël Naïm - Toxic - YouTube");
            Assert.AreEqual(URL.GetTitle(@"http://imgur.com/gallery/G2YpErL", true), "Title: Cows: shampooed, conditioned, and blow dried. - Imgur");
        }

        [Test]
        public void GetMimeSimple()
        {
            Assert.AreEqual(URL.GetTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-DVD.iso", true), "[application/x-iso9660-image 3.9 GiB]");
            Assert.AreEqual(URL.GetTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-KdeLive.iso", true), "[application/x-iso9660-image 1.2 GiB]");
            Assert.AreEqual(URL.GetTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-Minimal.iso", true), "[application/x-iso9660-image 566 MiB]");
            Assert.AreEqual(URL.GetTitle(@"http://php.net/images/logo.php", true), "[image/png 1.4 KiB]");
        }

        [Test]
        public void ToggleTitlePrint()
        {
            Assert.AreEqual(URL.GetTitle(@"http://www.google.com", false), "");
            Assert.AreEqual(URL.GetTitle(@"http://www.mirrorservice.org/sites/mirror.centos.org/7/isos/x86_64/CentOS-7.0-1406-x86_64-DVD.iso", false), "[application/x-iso9660-image 3.9 GiB]");
        }

        [Test]
        public void GetTitleUASpoof()
        {
            Assert.AreEqual(URL.GetTitle(@"http://tinkering.graymalk.in/ircbot/test-ua.php", true), "Title: User Agent: Mozilla/5.0 (MSIE 9.0; Windows NT 6.1; Trident/5.0)");
        }

        [Test]
        public void GetURL()
        {
            string[] k = { "http://www.flickr.com/photos/phaseviii/9217207106/in/set-72157626056708495" };
            List<String> m = ThreadedIRCBot.URL.GetURLs(k);

            Assert.AreEqual(k[0], m[0]);
        }
    }
}
