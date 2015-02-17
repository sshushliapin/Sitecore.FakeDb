namespace Sitecore.FakeDb.Tasks
{
  using Sitecore.Tasks;

  public class TaskDatabaseSwitcher : ThreadLocalProviderSwitcher<TaskDatabase>
  {
    public TaskDatabaseSwitcher(TaskDatabase provider)
      : base((IThreadLocalProvider<TaskDatabase>)Globals.TaskDatabase, provider)
    {
    }
  }
}