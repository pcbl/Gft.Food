using System.Collections.Generic;

namespace Gft.FoodMenu.Menus
{
    /// <summary>
    /// Describes the general behavior of a Menu
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// What Dish Options do we have??
        /// </summary>
        DishOption[] DishOptions { get; }

        /// <summary>
        /// What is the Name of the Menu?
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Processes the provided INPUT
        /// </summary>
        /// <param name="inputValues">List of items to be parsed and evaluated aginst the available Dish Options</param>
        /// <returns>Returns a plain string with the result of the processing</returns>
        string ProcessInput(IEnumerable<string> inputValues);
    }
}