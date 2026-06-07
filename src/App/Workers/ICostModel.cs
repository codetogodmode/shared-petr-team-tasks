namespace App.Workers;

/// <summary>
/// Seam pro výpočet ceny tasku. Současný (interim) model počítá cenu jen ze <see cref="Size"/>
/// jako baseline odhad. Budoucí implementace ji spočítá jako hodiny(Size, Seniority) × sazba(Worker).
/// PODHOUBÍ — bez implementace. Viz docs/specs/2026-06-07-workers-and-costing.md
/// </summary>
interface ICostModel
{
    /// <summary>Odhad hodin pro daný Size a senioritu vykonavatele (senior = méně hodin).</summary>
    decimal EstimateHours(Size size, Seniority seniority);

    /// <summary>Hodinová sazba pro daného workera (odvozená od seniority, příp. pozice).</summary>
    decimal HourlyRate(Worker worker);

    /// <summary>Výsledná cena tasku při přiřazení konkrétního workera.</summary>
    decimal CostFor(Size size, Worker worker);
}
