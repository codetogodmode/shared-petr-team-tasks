namespace App.Workers;

/// <summary>
/// Seniorita pracovníka. Určuje hodinovou sazbu i počet hodin potřebných na daný <see cref="Size"/>.
/// Pravidlo: vyšší seniorita = méně hodin, ale vyšší sazba.
/// PODHOUBÍ — záměrně bez logiky. Viz docs/specs/2026-06-07-workers-and-costing.md
/// </summary>
enum Seniority
{
    Junior,
    Medior,
    Senior
}
