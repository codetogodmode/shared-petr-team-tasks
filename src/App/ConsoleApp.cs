namespace App;

/// <summary>
/// Konzolové UI pro projektový management nad jedním projektem. Záměrně tenká vrstva:
/// čte vstup, volá doménu (Project/TaskItem) a vypisuje výstup. Žádná byznys logika tady nežije.
/// </summary>
class ConsoleApp
{
    private readonly Project _project;

    public ConsoleApp(Project project) => _project = project;

    public void Run()
    {
        while (true)
        {
            PrintMenu();
            var choice = Console.ReadLine();
            if (choice is null) return; // konec vstupu (např. konec piped streamu)

            Console.WriteLine();
            switch (choice.Trim())
            {
                case "1": AddTask(); break;
                case "2": AssignTask(); break;
                case "3": CompleteTask(); break;
                case "4": ListTasks(); break;
                case "5": ShowStatistics(); break;
                case "0": return;
                default: Console.WriteLine("Neplatná volba, zkus to znovu."); break;
            }
            Console.WriteLine();
        }
    }

    private void PrintMenu()
    {
        Console.WriteLine($"--- {_project.Name} | zbývá {_project.RemainingBudget:N0} Kč ---");
        Console.WriteLine("1) Přidat task");
        Console.WriteLine("2) Přiřadit task osobě");
        Console.WriteLine("3) Označit task hotový");
        Console.WriteLine("4) Vypsat tasky");
        Console.WriteLine("5) Statistiky");
        Console.WriteLine("0) Konec");
        Console.Write("Volba: ");
    }

    private void AddTask()
    {
        var title = Ask("Název tasku");
        if (title.Length == 0)
        {
            Console.WriteLine("Název nesmí být prázdný.");
            return;
        }

        if (!TryAskSize(out var size))
            return;

        var task = new TaskItem(title, size);
        _project.AddTask(task);

        Console.WriteLine($"Přidán '{task.Title}' ({size}) za {task.Cost:N0} Kč. Zbývá {_project.RemainingBudget:N0} Kč.");
        if (_project.RemainingBudget < 0)
            Console.WriteLine($"⚠ Rozpočet přečerpán o {-_project.RemainingBudget:N0} Kč!");
    }

    private void AssignTask()
    {
        if (!TryPickTask("přiřadit", out var task))
            return;

        var person = Ask("Jméno osoby");
        try
        {
            task.AssignTo(person);
            Console.WriteLine($"Task '{task.Title}' přiřazen: {task.AssignedTo}.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Nelze přiřadit: {ex.Message}.");
        }
    }

    private void CompleteTask()
    {
        if (!TryPickTask("dokončit", out var task))
            return;

        try
        {
            task.Complete();
            Console.WriteLine($"Task '{task.Title}' označen jako hotový.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message + ".");
        }
    }

    private void ListTasks()
    {
        if (_project.Tasks.Count == 0)
        {
            Console.WriteLine("Zatím žádné tasky.");
            return;
        }

        Console.WriteLine($"{"#",-3} {"Název",-25} {"Size",-4} {"Cena",10}  {"Stav",-10} {"Přiřazeno",-15}");
        Console.WriteLine(new string('-', 74));
        for (var i = 0; i < _project.Tasks.Count; i++)
        {
            var t = _project.Tasks[i];
            Console.WriteLine($"{i + 1,-3} {Truncate(t.Title, 25),-25} {t.Size,-4} {t.Cost,10:N0}  {t.Status,-10} {t.AssignedTo ?? "—",-15}");
        }
    }

    private void ShowStatistics()
    {
        var tasks = _project.Tasks;
        Console.WriteLine($"=== Statistiky: {_project.Name} ===");

        // D1 — rozpočet
        Console.WriteLine($"Rozpočet: celkem {_project.Budget:N0} | alokováno {_project.AllocatedCost:N0} | zbývá {_project.RemainingBudget:N0} Kč");
        if (_project.RemainingBudget < 0)
            Console.WriteLine($"⚠ Přečerpáno o {-_project.RemainingBudget:N0} Kč.");

        // D2 — stav
        var completed = tasks.Count(t => t.Status == TaskStatus.Completed);
        var pct = tasks.Count == 0 ? 0 : 100.0 * completed / tasks.Count;
        Console.WriteLine($"Tasky: {tasks.Count} (hotovo {completed}, čeká {tasks.Count - completed}) — {pct:0.#}% dokončeno");

        // D3 — rozložení podle velikosti
        if (tasks.Count > 0)
        {
            var bySize = tasks.GroupBy(t => t.Size)
                .OrderBy(g => g.Key)
                .Select(g => $"{g.Key}:{g.Count()}");
            Console.WriteLine($"Podle velikosti: {string.Join(", ", bySize)}");
        }

        // D4 — workload podle osoby
        var byPerson = tasks.Where(t => t.AssignedTo is not null)
            .GroupBy(t => t.AssignedTo!)
            .OrderByDescending(g => g.Sum(t => t.Cost))
            .ToList();
        if (byPerson.Count > 0)
        {
            Console.WriteLine("Workload:");
            foreach (var g in byPerson)
                Console.WriteLine($"  {g.Key}: {g.Count()} tasků, {g.Sum(t => t.Cost):N0} Kč");
        }
    }

    // --- pomocné metody pro vstup ---

    private static string Ask(string label)
    {
        Console.Write($"{label}: ");
        return (Console.ReadLine() ?? "").Trim();
    }

    private static bool TryAskSize(out Size size)
    {
        Console.Write("Velikost (XS/S/M/L/XL): ");
        var input = (Console.ReadLine() ?? "").Trim();
        if (Enum.TryParse(input, ignoreCase: true, out size) && Enum.IsDefined(size))
            return true;

        Console.WriteLine("Neplatná velikost.");
        return false;
    }

    private bool TryPickTask(string action, out TaskItem task)
    {
        task = null!;
        if (_project.Tasks.Count == 0)
        {
            Console.WriteLine("Zatím žádné tasky.");
            return false;
        }

        ListTasks();
        Console.Write($"Číslo tasku k {action}: ");
        var input = (Console.ReadLine() ?? "").Trim();
        if (int.TryParse(input, out var index) && index >= 1 && index <= _project.Tasks.Count)
        {
            task = _project.Tasks[index - 1];
            return true;
        }

        Console.WriteLine("Neplatné číslo tasku.");
        return false;
    }

    private static string Truncate(string value, int max)
        => value.Length <= max ? value : value[..(max - 1)] + "…";
}
