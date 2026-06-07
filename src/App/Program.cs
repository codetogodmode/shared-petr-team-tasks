namespace App;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Team Tasks — projektový management ===");
        Console.WriteLine();

        var project = CreateProject();
        if (project is null)
            return; // konec vstupu během zakládání

        new ConsoleApp(project).Run();
        Console.WriteLine("Nashledanou.");
    }

    /// <summary>Založí projekt; opakuje dotaz, dokud vstup neprojde validací. Vrací null při konci vstupu.</summary>
    private static Project? CreateProject()
    {
        while (true)
        {
            Console.Write("Název projektu: ");
            var name = Console.ReadLine();
            if (name is null) return null;

            Console.Write("Rozpočet (Kč): ");
            var budgetInput = Console.ReadLine();
            if (budgetInput is null) return null;

            if (!decimal.TryParse(budgetInput.Trim(), out var budget))
            {
                Console.WriteLine("Rozpočet musí být číslo. Zkus to znovu.\n");
                continue;
            }

            try
            {
                return new Project(name.Trim(), budget);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"{ex.Message}. Zkus to znovu.\n");
            }
        }
    }
}
