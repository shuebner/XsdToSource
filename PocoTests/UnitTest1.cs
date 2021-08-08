using NUnit.Framework;

namespace PocoTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var pocoInstance = new MyRootElement();
        }
    }
}