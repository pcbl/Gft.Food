using Gft.FoodMenu.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gft.FoodMenu
{
    /// <summary>
    /// Class that manages the Menu.
    /// We have used the Singleton Design Pattern to control access to this class and force the existance of only one instance for the whole application
    /// </summary>
    public class MenuManager
    {
        #region Singleton Implementation
        private static MenuManager _instance;
        public static MenuManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MenuManager();
                return _instance;
            }
        }
        private MenuManager()
        {
            InitializeMenusViaMEF(CUSTOMMENUSFOLDER);            
        }
        #endregion

        #region Menus Loading via MEF
        /// <summary>
        /// Menu Options, lazily populated via MEF
        /// </summary>
        public IEnumerable<Lazy<IMenu>> AvailableMenus { get; private set; }

        /// <summary>
        /// Custom Menus Folder Name
        /// </summary>
        public const string CUSTOMMENUSFOLDER="CustomMenus";

        /// <summary>
        /// Initializes the MEF engine(called only once from the Singleton Constructor)
        /// </summary>
        /// <param name="customMenusFolderName">Folder Name to check for custom Menus</param>
        private void InitializeMenusViaMEF(string customMenusFolderName)
        {
            try
            {
                //We will use MEF to load all available Menus, aggregating from 2 sources:                       
                using (var catalog = new System.ComponentModel.Composition.Hosting.AggregateCatalog())
                {
                    //1 - Main application assembly
                    catalog.Catalogs.Add(new System.ComponentModel.Composition.Hosting.AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()));
                    //2 - customMenusFolderName(default is CustomMenus) directory(only if it exists) on same location as the CORE assembly is running
                    var customMenusFullPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), customMenusFolderName);
                    if (System.IO.Directory.Exists(customMenusFullPath))
                        catalog.Catalogs.Add(new System.ComponentModel.Composition.Hosting.DirectoryCatalog(customMenusFullPath));

                    //Then we finally compose our AvailableMenus attribute via the MEF composition engine
                    var container = new System.ComponentModel.Composition.Hosting.CompositionContainer(catalog);
                    AvailableMenus = container.GetExports<IMenu>();
                }
            }
            catch (Exception ex)
            {
                //If somethiong wrong happens here we just forward the exception with some relevant information
                throw new InvalidOperationException(string.Format("Error while Locating Available Menus: {0}",ex.Message), ex);
            }
        }
        #endregion

        #region Attributes and Constants
        /// <summary>
        /// 'error' Constant, used to show the user that something went wrong
        /// </summary>
        public const string ERROR = "error";
        #endregion

        /// <summary>
        /// Process the given order Request
        /// </summary>
        /// <param name="input">Input to be evaluated</param>
        /// <returns>Returns the processing result. The result varies on the used Menu
        /// Ex:
        ///  morning:
        ///      -1=eggs(entree)
        ///      -2=toast(side)
        ///      -3=coffee(drink)(Multiple Orders Supported)
        ///  INPUT: morning,1,2,3,3
        ///  OUTPUT: eggs, toast, coffee(x2)
        ///  INPUT: morning,3,2,3,3
        ///  OUTPUT: toast, coffee(x3)
        /// </returns>
        public string ProcessOrderRequest(string input)
        {
            //First lets be sure the user provided a valid input
            if(!string.IsNullOrWhiteSpace(input))
            {
                //Then, Lets split it and Locate the proper Menu
                //We expect that the splittedInput[0] has a valid menu name
                var splittedInput = input.Split(',');
                var foundMenu = SelectMenu(splittedInput[0]);
                //If no menu was found, we return error
                if (foundMenu != null)
                    //Now we have the Menu, so we can delegate the task of finding out the proper output
                    //To do so, we skip the first element as we already used it on previous step
                    return foundMenu.Value.ProcessInput(splittedInput.Skip(1));
                else
                    //No Menu found! Return error!
                    return ERROR;
            }
            else //No Input provided! Return error
                return ERROR;
        }

        /// <summary>
        /// Looks for a Menu inside the AvailableMenus Collection
        /// </summary>
        /// <param name="menuName">Menu name to look for </param>
        /// <returns>Returns the found Menu, otherwise, will return null.</returns>
        private Lazy<IMenu> SelectMenu(string menuName)
        {
            //We use LINQ´s FirstOrDefault method to compare the menu name among the available ones
            //PLEASE NOTE THAT THE METHOD IS CASE INSENSITIVE
            return AvailableMenus.FirstOrDefault(item => string.Compare(item.Value.Name, menuName, true) == 0);
        }

    }
}
