namespace App.Tests;

public class TaskItemTests
{
    [Fact]
    public void Cost_is_derived_from_size_via_baseline_table()
    {
        Assert.Equal(2000m, new TaskItem("Task", Size.XS).Cost);
        Assert.Equal(4000m, new TaskItem("Task", Size.S).Cost);
        Assert.Equal(8000m, new TaskItem("Task", Size.M).Cost);
        Assert.Equal(16000m, new TaskItem("Task", Size.L).Cost);
        Assert.Equal(32000m, new TaskItem("Task", Size.XL).Cost);
    }

    [Fact]
    public void Complete_marks_the_task_completed()
    {
        var task = new TaskItem("Task", Size.S);
        task.Complete();
        Assert.Equal(TaskStatus.Completed, task.Status);
    }

    [Fact]
    public void Completing_an_already_completed_task_throws()
    {
        var task = new TaskItem("Task", Size.S);
        task.Complete();
        Assert.Throws<InvalidOperationException>(() => task.Complete());
    }

    [Fact]
    public void AssignTo_sets_the_assignee()
    {
        var task = new TaskItem("Task", Size.S);
        task.AssignTo("Petr");
        Assert.Equal("Petr", task.AssignedTo);
    }

    [Fact]
    public void AssignTo_rejects_blank_name()
    {
        var task = new TaskItem("Task", Size.S);
        Assert.Throws<ArgumentException>(() => task.AssignTo("   "));
    }
}
