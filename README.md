# .NET Developer Practicum

**The .NET Developer practicum is evaluated on:**

1. Object Oriented Design
2. Readability
3. Maintainability
4. Testability

**Technical Requirements:**

1. Create this solution as a console application
2. Solution must have unit tests
3. Solution must have a build script that can compile and test the solution from the command line
4. Put your solution in a GitHub repository, and send us a link when done

**Rules:**

1. You must enter time of day as "morning" or "night"
2. You must enter a comma delimited list of dish types with at least one selection
3. The output must print food in the following order: entrée, side, drink, dessert
4. There is no dessert for morning meals
5. Input is not case sensitive
6. If invalid selection is encountered, display valid selections up to the error, then print error
7. In the morning, you can order multiple cups of coffee
8. At night, you can have multiple orders of potatoes
9. Except for the above rules, you can only order 1 of each dish type

**Dishes for Each time of day**

| Dish Type | Morning | night |
| --- | --- | --- |
| 1 (entrée) | Eggs | steak |
| 2 (side) | Toast | potato |
| 3 (drink) | Coffee | wine |
| 4 (dessert) | _Not Applicable_ | cake |

**Sample Input and Output:**


**Input:** morning, 1, 2, 3

**Output:** eggs, toast, coffee

---

**Input:** morning, 2, 1, 3

**Output:** eggs, toast, coffee

---


**Input:** morning, 1, 2, 3, 4

**Output:** eggs, toast, coffee, error

---


**Input:** morning, 1, 2, 3, 3, 3

**Output:** eggs, toast, coffee(x3)

---


**Input:** night, 1, 2, 3, 4

**Output:**  steak, potato, wine, cake

---


**Input:** night, 1, 2, 2, 4

**Output:** steak, potato(x2), cake


# Implementation

This git repository contains a solution ('Gft.FoodMenu.sln' on root folder) for this Practicum. It contains the following projects:

| Project | Description |
| --- | --- | 
| Gft.FoodMenu.Client | Entrypoint to the executable. Simple console application. | 
| Gft.FoodMenu.Core | Class library that holds the core logic of the Practicum | 
| Gft.FoodMenu.Tests | Unit Tests Project, with 100% Code coverage of Gft.FoodMenu.Core | 

The solution was designed to support additional **custom** menus by using MEF. All you have to do is:

1. Create your Class Library
2. Add Reference to(Gft.FoodMenu.Core.dll)
3. Inherit from Gft.FoodMenu.Menus.BaseMenu Abstract Class
4. Implement Name Property and GetDishOptions() method
5. Add Reference to .NET Framework's MEF dll, System.ComponentModel.Composition.dll
6. Decorate your custom Menu Class with the Export Attribute ([Export(typeof(IMenu))])
7. Compile your Class Library
8. COoy your class Library dll to a subfolder named "CustomMenus" within the same directory as the Gft.FoodMenu.Client.exe file. **If this folder does not exists, you will have to create it.**

**SampleImplementation of a BaseMenu**
```cSharp
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
```


**Powershell script**

As requested, a Powershell Script that compiles and run all Unit Tests from Command Line is given(BuildAndRunTests.ps1 on same level as the solution(.sln) file).
To call it just Open Visual Studio Command Prompt, go to the Solution Folder and Type this:

```
powershell -file .\BuildAndRunTests.ps1
```

The results will be within the BuildResults folder.

#Have fun...
