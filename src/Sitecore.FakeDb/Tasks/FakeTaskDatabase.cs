namespace Sitecore.FakeDb.Tasks
{
  using System;
  using System.Threading;
  using Sitecore.Data.Items;
  using Sitecore.Tasks;

  public class FakeTaskDatabase : TaskDatabase, IThreadLocalProvider<TaskDatabase>
  {
    private readonly ThreadLocal<TaskDatabase> localProvider = new ThreadLocal<TaskDatabase>();

    private readonly Task[] emptyTasks = { };

    private bool disposed;

    public virtual ThreadLocal<TaskDatabase> LocalProvider
    {
      get { return this.localProvider; }
    }

    public override void Disable(Task task)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.Disable(task);
      }
    }

    public override void Enable(Task task)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.Enable(task);
      }
    }

    public override Task[] GetPendingTasks()
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetPendingTasks() : this.emptyTasks;
    }

    public virtual bool IsLocalProviderSet()
    {
      return this.localProvider.Value != null;
    }

    public override void MarkDone(Task task)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.MarkDone(task);
      }
    }

    public override void Remove(Task task)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.Remove(task);
      }
    }

    public override void RemoveItemTasks(Item item)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.RemoveItemTasks(item);
      }
    }

    public override void RemoveItemTasks(Item item, Type taskType)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.RemoveItemTasks(item, taskType);
      }
    }

    public override void Update(Task task, bool insertIfNotFound)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.Update(task, insertIfNotFound);
      }
    }

    public override void UpdateItemTask(Task task, bool insertIfNotFound)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.UpdateItemTask(task, insertIfNotFound);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (!disposing)
      {
        return;
      }

      this.localProvider.Dispose();

      this.disposed = true;
    }
  }
}