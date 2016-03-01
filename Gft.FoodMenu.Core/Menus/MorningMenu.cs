namespace Gft.FoodMenu.Menus
{
    /// <summary>
    /// Concrete Implementation, in charge the so called Morning Menu.
    /// PLEASE NOTE THE Export Attribute, used by MEF COmposition Engine
    /// </summary>
    [System.ComponentModel.Composition.Export(typeof(IMenu))]
    public class MorningMenu : BaseMenu
    {
        public override string Name
        {
            get
            {
                return "morning";
            }
        }

        protected override DishOption[] GetDishOptions()
        {
            return new DishOption[]
            {
                new DishOption("eggs",   DishTypes.entree),
                new DishOption("toast",  DishTypes.side),
                new DishOption("coffee", DishTypes.drink, supportMultipleOrders:true),
            };
        }
    }
}
