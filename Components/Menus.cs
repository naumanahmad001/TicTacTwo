namespace Components;

public class Menus
{
    public static void PrintFirstMenu()
    {    
        Console.WriteLine("|  <<<<< TIC-TAC-TWO >>>>>> |");
        Console.WriteLine("|───────────────────────────|");
        Console.WriteLine("|       New game (N)        |");
        Console.WriteLine("|       Load game (L)       |");
        Console.WriteLine("|       Exit (E)            |");
        Console.WriteLine("|                           |");
        Console.WriteLine("|───────────────────────────|");
        
    }

    public static void PrintSecondMenu()
    {
        Console.Clear();
        Console.WriteLine("|───────────────────────────|");
        Console.WriteLine("|    Set configuration (S)  |");
        Console.WriteLine("|   Load Configuration (L)  |");
        Console.WriteLine("|       Back(B)             |");
        Console.WriteLine("|       Exit (E)            |");
        Console.WriteLine("|                           |");
        Console.WriteLine("|───────────────────────────|");
    }
    
}