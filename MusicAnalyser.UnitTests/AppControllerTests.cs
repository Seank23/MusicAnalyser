using NUnit.Framework;

namespace MusicAnalyser.UnitTests
{
    public class AppControllerTests
    {
        [SetUp]
        public void Setup()
        {
            Form1 frm = new Form1();
            AppController app = new AppController(frm);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}