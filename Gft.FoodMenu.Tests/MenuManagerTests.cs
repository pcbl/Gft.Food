using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gft.FoodMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gft.FoodMenu.Tests
{
    [TestClass()]
    public class MenuManagerTests
    {
        #region ProcessOrderRequest Tests
        [TestMethod()]
        public void ProcessOrderRequest_EmptyEntryTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest(string.Empty);
            Assert.AreEqual("error", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_InvalidEntryTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest(",2,3,4");
            Assert.AreEqual("error", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_NoDishesEntryTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("morning");
            Assert.AreEqual("error", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_Morning_InvalidDishTypeTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("morning,1,ab");
            Assert.AreEqual("eggs, error", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_Morning_SimpleTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("morning, 1, 2, 3");
            Assert.AreEqual("eggs, toast, coffee", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_Morning_UnorderedTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("morning, 2, 1, 3");
            Assert.AreEqual("eggs, toast, coffee", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_Morning_DesertTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("morning, 1, 2, 3, 4");
            Assert.AreEqual("eggs, toast, coffee, error", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_Morning_TreeCofeesTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("morning, 1, 2, 3, 3, 3");
            Assert.AreEqual("eggs, toast, coffee(x3)", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_Morning_TwoEggsTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("morning, 1, 1, 2, 3");
            Assert.AreEqual("error", result);
        }


        [TestMethod()]
        public void ProcessOrderRequest_Morning_TwoToastsTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("morning, 1, 2, 2, 3");
            Assert.AreEqual("eggs, error", result);
        }
        
        [TestMethod()]
        public void ProcessOrderRequest_Night_SimpleTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("night, 1, 2, 3, 4");
            Assert.AreEqual("steak, potato, wine, cake", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_Night_FivePotatoesTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("night,2, 1, 2, 2, 4, 2, 2");
            Assert.AreEqual("steak, potato(x5), cake", result);
        }

        [TestMethod()]
        public void ProcessOrderRequest_Night_TreeWinesTest()
        {
            var result = MenuManager.Instance.ProcessOrderRequest("night, 1, 2, 3, 3, 3, 4");
            Assert.AreEqual("steak, potato, error", result);
        }
        #endregion

        #region MEF Testing
        /// <summary>
        /// For MEF Loading Test we will add the Unit Tests Assembly to the CUSTOMMENUSFOLDER
        /// AfternoonMenu then should be available
        /// </summary>
        /// <param name="context">Test Context</param>
        [AssemblyInitialize()]
        public static void SetupForMEF(TestContext context)
        {
            var testsAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //We shall copy the Unit Test Dll to the CUSTOMMENUSFOLDER Folder to test the MEF loading!
            var targetMEFDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(testsAssemblyPath), MenuManager.CUSTOMMENUSFOLDER);
            if (!System.IO.Directory.Exists(targetMEFDirectory))
                System.IO.Directory.CreateDirectory(targetMEFDirectory);            
            var targetFile = System.IO.Path.Combine(targetMEFDirectory, System.IO.Path.GetFileName(testsAssemblyPath));
            if (!System.IO.File.Exists(targetFile)) 
                System.IO.File.Copy(testsAssemblyPath, targetFile);
        }

        /// <summary>
        /// Once the tests ran we just delete the folder to avoid unexpected behavior when debuggint the client app
        /// </summary>
        [AssemblyCleanup()]
        public static void CleanupForMEF()
        {            
            var testsAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var targetMEFDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(testsAssemblyPath), MenuManager.CUSTOMMENUSFOLDER);
            if (System.IO.Directory.Exists(targetMEFDirectory))
                System.IO.Directory.Delete(targetMEFDirectory,true);
        }

        [TestMethod()]
        public void TestCustomMenusMEFLoading()
        {
            //We should just assure that the Afternoon is avaiable(Just for testing purpose!)
            Assert.IsTrue(MenuManager.Instance.AvailableMenus.Any(item => string.Compare(item.Value.Name, "Afternoon")==0),"Afternoon was not loaded");
        }


        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestMEFExceptionHandling()
        {
            PrivateObject obj = new PrivateObject(typeof(MenuManager));
            string nullDirectory=null;
            var forcePopulationOfIt = obj.Invoke("InitializeMenusViaMEF", nullDirectory);
        }
        #endregion
    }
}