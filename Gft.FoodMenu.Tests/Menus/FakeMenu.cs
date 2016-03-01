namespace Gft.FoodMenu.Menus.Tests
{
    /// <summary>
    /// Fake Class, created to test the BaseMenu Constructor Validation Check
    /// Refer to BaseMenuTests.ConstructorErrorTest for more info
    /// </summary>
    internal class FakeMenu : BaseMenu
    {
        public override string Name
        {
            get
            {
                return "ToTest";
            }
        }

        protected override DishOption[] GetDishOptions()
        {
            return null;
        }
    }
}
