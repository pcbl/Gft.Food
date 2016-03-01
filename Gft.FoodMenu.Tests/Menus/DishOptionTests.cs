using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Gft.FoodMenu.Menus.Tests
{
    [TestClass()]
    public class DishOptionTests
    {
        #region IComparable<> Tests
        [TestMethod()]
        public void CompareToTest()
        {
            var optionA = new DishOption("Test Entreé", DishTypes.entree);
            var optionB = new DishOption("Test Side", DishTypes.side);
            var optionC = new DishOption("Test Drink", DishTypes.drink);
            var optionD = new DishOption("Test Dessert", DishTypes.dessert);
            List<DishOption> toSort = new List<DishOption>(new []{  optionA, optionD, optionC, optionB});
            List<DishOption> alreadySorted = new List<DishOption>(new[] { optionA, optionB, optionC, optionD});
            toSort.Sort();
            CollectionAssert.AreEqual(alreadySorted,toSort, "ICOmparable<DishOption> failed");
        }
        
        [TestMethod()]
        public void CompareNullTest()
        {
            var optionA = new DishOption("Test Entreé", DishTypes.entree);
            var optionB = new DishOption("Test Drink", DishTypes.side);
            List<DishOption> toSort = new List<DishOption>(new[] { null, optionB, null, optionA });
            List<DishOption> alreadySorted = new List<DishOption>(new[] { null, null, optionA, optionB });
            toSort.Sort();
            CollectionAssert.AreEqual(alreadySorted, toSort, "ICOmparable<DishOption>(including Null values) failed");
        }
        #endregion
    }
}