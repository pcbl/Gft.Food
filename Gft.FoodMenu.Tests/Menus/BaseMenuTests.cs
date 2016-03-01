using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gft.FoodMenu.Menus.Tests
{
    [TestClass()]
    public class BaseMenuTests
    {
        #region Constructore Test
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorErrorTest()
        {
            //This will trigger an InvalidOperationException as we have no DishOptions on the Fake one
            var fakeTest = new FakeMenu();
        }
        #endregion

        #region ProcessInput direct call invalid values Test
        [TestMethod()]        
        public void ProcessInputNullTest()
        {
            AfternoonMenu menu = new AfternoonMenu();
            Assert.AreEqual("error",menu.ProcessInput(null));
        }

        [TestMethod()]
        public void ProcessInputEmptyTest()
        {
            AfternoonMenu menu = new AfternoonMenu();
            Assert.AreEqual("error", menu.ProcessInput(new string[] { }));
        }
        #endregion
    }
}