namespace Sitecore.FakeDb.Tasks
{
  using System;
  using System.Threading;
  using Sitecore.Data.Items;
  using Sitecore.Tasks;

  public class FakeTaskDatabase : TaskDatabase, IBehavioral<TaskDatabase>
  {
    private readonly ThreadLocal<TaskDatabase> behavior = new ThreadLocal<TaskDatabase>();

    private readonly StubTaskDatabase stub = new StubTaskDatabase();

    public TaskDatabase Behavior
    {
      get { return this.behavior.Value ?? this.stub; }
      set { this.behavior.Value = value; }
    }

    public override void Disable(Task task)
    {
      this.Behavior.Disable(task);
    }

    public override void Enable(Task task)
    {
      this.Behavior.Enable(task);
    }

    public override Task[] GetPendingTasks()
    {
      return this.Behavior.GetPendingTasks();
    }

    public override void MarkDone(Task task)
    {
      this.Behavior.MarkDone(task);
    }

    public override void Remove(Task task)
    {
      this.Behavior.Remove(task);
    }

    public override void RemoveItemTasks(Item item)
    {
      this.Behavior.RemoveItemTasks(item);
    }

    public override void RemoveItemTasks(Item item, Type taskType)
    {
      this.Behavior.RemoveItemTasks(item, taskType);
    }

    public override void Update(Task task, bool insertIfNotFound)
    {
      this.Behavior.Update(task, insertIfNotFound);
    }

    public override void UpdateItemTask(Task task, bool insertIfNotFound)
    {
      this.Behavior.UpdateItemTask(task, insertIfNotFound);
    }
  }
}