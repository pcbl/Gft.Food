using System;

namespace Gft.FoodMenu.Menus
{
    /// <summary>
    /// Represents a Dish Option, available within a Menu
    /// PLEASE NOTE THE IComparable<> IMPLEMENTATION, REQUIRED TO PERFORM THE SORTING OF LISTS OF DISHOPTION CLASS
    /// </summary>
    public class DishOption:IComparable<DishOption>
    {
        /// <summary>
        /// Creates an instance of a Dish Option
        /// </summary>
        /// <param name="name">Name of the dish</param>
        /// <param name="dishType">Type of the dish(according to the DishTypes enum)</param>
        /// <param name="supportMultipleOrders">OPTIONAL. Indicates if the Dish option can be ordered Multiple Times or Not. DEfault is false.</param>
        public DishOption(string name, DishTypes dishType, bool supportMultipleOrders=false)
        {
            Name = name;
            SupportsMultipleOrders = supportMultipleOrders;
            DishType = dishType;
        }

        /// <summary>
        /// Type of the dish(according to the DishTypes enum)
        /// </summary>
        public DishTypes DishType { get; private set; }

        /// <summary>
        /// Name of the dish
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Indicates if the Dish option can be ordered Multiple Times or Not. 
        /// </summary>
        public bool SupportsMultipleOrders { get; private set; }

        #region IComparable implementation
        /// <summary>
        /// From IComparable<>, used to sort lists of Dish Options based on the DishType enum
        /// </summary>
        /// <param name="other">The other part to compare</param>
        /// <returns>Negative if it should come before the given element, positive if it should come after the provided element</returns>
        public int CompareTo(DishOption other)
        {
            if (!object.ReferenceEquals(other, null))
                return this.DishType.CompareTo(other.DishType);
            else
                return 1;//When we have Null, it comes first
        }
        #endregion
    }
}
