using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [ClassInitialize]
        public void TestInit()
        {
            System.Diagnostics.Debug.WriteLine("Init");
        }

        [ClassCleanup]
        public void TestClean()
        {
            System.Diagnostics.Debug.WriteLine("Clean");
        }

        [TestMethod]
        public void TestMethodA()
        {
            System.Diagnostics.Debug.WriteLine("A");
        }

        [TestMethod]
        public void TestMethodB()
        {
            System.Diagnostics.Debug.WriteLine("B");
        }
    }
}
