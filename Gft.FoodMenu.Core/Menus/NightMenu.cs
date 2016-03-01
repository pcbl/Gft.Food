using System.ComponentModel.Composition;

namespace Gft.FoodMenu.Menus
{
    /// <summary>
    /// Concrete Implementation, in charge the so called Night Menu.
    /// PLEASE NOTE THE Export Attribute, used by MEF COmposition Engine
    /// </summary>
    [Export(typeof(IMenu))]
    public class NightMenu:BaseMenu
    {
        public override string Name
        {
            get
            {
                return "night";
            }
        }

        protected override DishOption[] GetDishOptions()
        {
            return new DishOption[]
            {
                new DishOption("steak",  DishTypes.entree),
                new DishOption("potato", DishTypes.side, supportMultipleOrders:true),
                new DishOption("wine",   DishTypes.drink),
                new DishOption("cake",   DishTypes.dessert)
            };
        }
    }
}
