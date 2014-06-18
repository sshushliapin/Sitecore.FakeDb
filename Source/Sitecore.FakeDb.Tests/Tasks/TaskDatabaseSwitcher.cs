namespace Sitecore.FakeDb.Tests.Tasks
{
  using Sitecore.FakeDb.Tasks;
  using Sitecore.Tasks;

  public class TaskDatabaseSwitcher : ProviderBehaviorSwitcher<TaskDatabase>
  {
    public TaskDatabaseSwitcher(TaskDatabase behavior)
      : base((FakeTaskDatabase)Globals.TaskDatabase, behavior)
    {
    }
  }
}