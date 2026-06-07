namespace App.Tests;

public class ProjectTests
{
    [Fact]
    public void New_project_has_no_allocation_and_full_remaining_budget()
    {
        var project = new Project("Web", 100_000m);
        Assert.Equal(0m, project.AllocatedCost);
        Assert.Equal(100_000m, project.RemainingBudget);
    }

    [Fact]
    public void Adding_a_task_allocates_its_cost()
    {
        var project = new Project("Web", 100_000m);
        project.AddTask(new TaskItem("Medium task", Size.M)); // 8000
        Assert.Equal(8_000m, project.AllocatedCost);
        Assert.Equal(92_000m, project.RemainingBudget);
    }

    [Fact]
    public void Allocation_sums_across_multiple_tasks()
    {
        var project = new Project("Web", 100_000m);
        project.AddTask(new TaskItem("A", Size.S));  // 4000
        project.AddTask(new TaskItem("B", Size.L));  // 16000
        Assert.Equal(20_000m, project.AllocatedCost);
    }

    [Fact]
    public void Remaining_budget_may_go_negative_when_overallocated()
    {
        var project = new Project("Web", 5_000m);
        project.AddTask(new TaskItem("Big", Size.XL)); // 32000
        Assert.Equal(-27_000m, project.RemainingBudget); // povoleno, žádná výjimka
    }

    [Fact]
    public void Constructor_rejects_blank_name()
    {
        Assert.Throws<ArgumentException>(() => new Project("   ", 1_000m));
    }

    [Fact]
    public void Constructor_rejects_negative_budget()
    {
        Assert.Throws<ArgumentException>(() => new Project("Web", -1m));
    }
}
