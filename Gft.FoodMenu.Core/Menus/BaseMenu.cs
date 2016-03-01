using System;
using System.Collections.Generic;
using System.Linq;

namespace Gft.FoodMenu.Menus
{
    /// <summary>
    /// Abstract Class that defines the basic behavior of a Menu
    /// </summary>
    public abstract class BaseMenu : IMenu
    {
        #region Abstract Methods/Attributes
        /// <summary>
        /// We let the Name abstract as it MUST be provided by classes that inherits from BaseMenu
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Method that defines the DishOptions within the Menu
        /// We implemented this method to call it only once(within the constructor)
        /// </summary>
        /// <returns>Returns an array of DishOptions within the Menu</returns>
        protected abstract DishOption[] GetDishOptions();
        #endregion

        #region Constructor and Attributes
        /// <summary>
        /// We actually implement a full property to load Dish Options only once(from the GetDishOptions abstract method)
        /// </summary>
        public DishOption[] DishOptions { get; private set; }
   
        /// <summary>
        /// Constructor, in charge to make some basic initialization of the Menu
        /// </summary>
        public BaseMenu()
        {
            //We call the abstract GetDishOptions method but we must be sure its return was OK!
            //If it is not OK we throw an exception righ away
            DishOptions = GetDishOptions();
            if (DishOptions == null || !DishOptions.Any())
                throw new ArgumentException("At least one dish option must be provided");
        }
        #endregion

        /// <summary>
        /// Processes the provided INPUT using the available DishOptions
        /// </summary>
        /// <param name="inputValues">List of items to be parsed and evaluated aginst the available Dish Options</param>
        /// <returns>Returns a plain string with the result of the processing</returns>
        public string ProcessInput(IEnumerable<string> inputValues)
        {
            //First of all we must assure the input is OK!
            if (inputValues!=null && inputValues.Any())
            {
                //Then we will iterate over the provided inputValues
                //And we will add the found Dish Options within the selectedDishOptions variable
                //Case an unexp0ected input was provided, we shall set the errorsOccurred variable,
                //and leave the loop immediatelly, as no more processing is further expected
                var selectedDishOptions = new List<DishOption>();
                bool errorsOccurred = false;
                foreach (var item in inputValues)
                {
                    int option;                    
                    //Must be an INT
                    if (int.TryParse(item, out option))
                    {
                        //Once we have a valid number, we should actually check if there is a Dish option available
                        //If it is OK we add it to the selectedDishOptions list
                        var foundDishOption = SelectDishTypeOption(option);
                        if (foundDishOption != null)
                        {
                            //If Dish Option was already added BUT
                            //the Dish Option does not supports it
                            //We have an error!
                            if(selectedDishOptions.Contains(foundDishOption) && 
                                !foundDishOption.SupportsMultipleOrders)
                            {
                                //We aditionally remove it from the list as the user will get an error message instead the item!
                                selectedDishOptions.Remove(foundDishOption);
                                errorsOccurred = true;
                                break;                                
                            }
                            else
                                //Otherwise, it is OK! Finally add it to the list
                                selectedDishOptions.Add(foundDishOption);
                        }
                        else
                        {//No Dish Option FOUND, set errorsOccurred and leave the loop
                            errorsOccurred = true;
                            break;
                        }
                    }
                    else
                    {//NOT a number! Set errorsOccurred and leave the loop
                        errorsOccurred = true;
                        break;
                    }
                }
                //Lets sort the list based on DishType(as defined on DishOption.IComparable<>)
                selectedDishOptions.Sort();
                //Once it is sorted, let's group the Items that repeated
                //This is important to add the multiplier number when appropriate                
                var groupedOptions = from dish in selectedDishOptions
                                     //We group by the type instance itself as we are using their instances
                                     group dish by dish into groupedDish                              
                                     select new
                                     {
                                         Dish = groupedDish.Key,
                                         Count = groupedDish.Count()
                                     };

                //Finally we iterate over the Grouped Options and prepare the result List
                List<string> result = new List<string>();
                foreach (var p in groupedOptions)
                    result.Add(p.Count > 1 ? string.Format("{0}(x{1})", p.Dish.Name, p.Count) : p.Dish.Name);
                //If errors occurred we just print Error and all processing is done!
                if (errorsOccurred)
                    result.Add(MenuManager.ERROR);
                //Then we return the items, comma separated
                return string.Join(", ", result);
            }
            else//Absolutelly no dish option was provided. return error on that case
                return MenuManager.ERROR;
        }

        /// <summary>
        /// Looks for Dish Option inside the DishOptions Collection
        /// </summary>
        /// <param name="dishType">Integer value that indicates the Dish Type to Look for</param>
        /// <returns>Returns the found DIsh Option Instance, otherwise, will return null.</returns>
        private DishOption SelectDishTypeOption(int dishType)
        {
            return DishOptions.FirstOrDefault(option => (int)option.DishType == dishType);
        }
    }
}
