namespace App.Workers;

/// <summary>
/// Pracovník, který může plnit tasky. Nahradí současný string <c>TaskItem.AssignedTo</c>.
/// PODHOUBÍ — datový tvar bez chování. Výpočet ceny patří do <see cref="ICostModel"/>.
/// Viz docs/specs/2026-06-07-workers-and-costing.md
/// </summary>
class Worker
{
    public string Name { get; }
    public Position Position { get; }
    public Seniority Seniority { get; }

    public Worker(string name, Position position, Seniority seniority)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Jméno pracovníka nesmí být prázdné");

        Name = name;
        Position = position;
        Seniority = seniority;
    }
}
