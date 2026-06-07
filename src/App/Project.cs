namespace App;

class Project
{
    public string Name { get; private set; }
    public decimal Budget { get; private set; }
    public List<TaskItem> Tasks { get; private set; } = new();
    public DateTime CreatedAt { get; private set; }

    /// <summary>Součet ceny všech tasků projektu (alokovaný rozpočet).</summary>
    public decimal AllocatedCost => Tasks.Sum(task => task.Cost);

    /// <summary>Zbývající rozpočet (Budget − AllocatedCost). Spočítaný — může být i záporný (přečerpáno).</summary>
    public decimal RemainingBudget => Budget - AllocatedCost;

    public Project(string name, decimal budget)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Jméno projektu nesmí být prázdné");
        if (budget < 0)
            throw new ArgumentException("Rozpočet nemůže být záporný");

        Name = name;
        Budget = budget;
        CreatedAt = DateTime.Now;
    }

    public void AddTask(TaskItem task)
    {
        Tasks.Add(task);
    }
}