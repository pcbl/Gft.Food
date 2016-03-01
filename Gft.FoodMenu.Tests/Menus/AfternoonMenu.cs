namespace Gft.FoodMenu.Menus.Tests
{
    /// <summary>
    /// Created just for MEF testing purposes!
    /// Please refer to MenuManagerTest.TestCustomMenusMEFLoading() 
    /// </summary>
    [System.ComponentModel.Composition.Export(typeof(IMenu))]
    public class AfternoonMenu : Gft.FoodMenu.Menus.BaseMenu
    {
        public override string Name
        {
            get
            {
                return "Afternoon";
            }
        }

        protected override DishOption[] GetDishOptions()
        {
            return new[] {
                new DishOption("Banana", DishTypes.entree),
                new DishOption("bread", DishTypes.side),
                new DishOption("Candy", DishTypes.dessert)
            };
        }
    }
}
