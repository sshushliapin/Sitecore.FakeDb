namespace Sitecore.FakeDb.Tasks
{
  using System;
  using Sitecore.Data.Items;
  using Sitecore.Tasks;

  public class StubTaskDatabase : TaskDatabase
  {
    private readonly Task[] getPendingTasks = new Task[0];

    public override void Disable(Task task)
    {
    }

    public override void Enable(Task task)
    {
    }

    public override Task[] GetPendingTasks()
    {
      return this.getPendingTasks;
    }

    public override void MarkDone(Task task)
    {
    }

    public override void Remove(Task task)
    {
    }

    public override void RemoveItemTasks(Item item)
    {
    }

    public override void RemoveItemTasks(Item item, Type taskType)
    {
    }

    public override void Update(Task task, bool insertIfNotFound)
    {
    }

    public override void UpdateItemTask(Task task, bool insertIfNotFound)
    {
    }
  }
}