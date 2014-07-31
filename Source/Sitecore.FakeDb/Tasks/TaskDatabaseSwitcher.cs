namespace Sitecore.FakeDb.Tasks
{
  using Sitecore.Tasks;

  public class TaskDatabaseSwitcher : ProviderBehaviorSwitcher<TaskDatabase>
  {
    public TaskDatabaseSwitcher(TaskDatabase provider)
      : base((FakeTaskDatabase)Globals.TaskDatabase, provider)
    {
    }
  }
}