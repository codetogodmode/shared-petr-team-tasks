namespace App;

class TaskItem
{
    public string Title { get; set; }
    public DateTime CreatedAt { get; private set; }
    public string? AssignedTo { get; private set; }
    public Size Size { get; set; }
    public TaskStatus Status { get; private set; }

    /// <summary>
    /// Baseline odhad ceny odvozený ze <see cref="Size"/>. Spočítaná vlastnost — vždy odráží aktuální Size.
    /// Po zavedení workerů ji nahradí výpočet hodiny × sazba (viz docs/specs/2026-06-07-workers-and-costing.md).
    /// </summary>
    public decimal Cost => BaselineCosts[Size];

    public TaskItem(string title, Size size)
    {
        Title = title;
        Size = size;
        Status = TaskStatus.Pending;
        CreatedAt = DateTime.Now;
    }

    public void Complete()
    {
        if (Status == TaskStatus.Completed)
            throw new InvalidOperationException("Task už je hotový");
        Status = TaskStatus.Completed;
    }

    public void AssignTo(string personName)
    {
        if (string.IsNullOrWhiteSpace(personName))
            throw new ArgumentException("Jméno nesmí být prázdné");
        AssignedTo = personName;
    }

    private static readonly Dictionary<Size, decimal> BaselineCosts = new()
    {
        [Size.XS] = 2000m,
        [Size.S] = 4000m,
        [Size.M] = 8000m,
        [Size.L] = 16000m,
        [Size.XL] = 32000m,
    };
}

enum TaskStatus { Pending, Completed }
enum Size { XS, S, M, L, XL }