namespace Sitecore.FakeDb.Tests.Tasks
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Tasks;
  using Sitecore.Tasks;
  using Xunit;

  public class StubTaskDatabaseTest
  {
    [Fact]
    public void ShouldBeQuiet()
    {
      // arrange
      var taskDatabase = new StubTaskDatabase();
      var task = Substitute.For<Task>(DateTime.Now);
      var item = ItemHelper.CreateInstance(Database.GetDatabase("master"));

      // act & assert
      taskDatabase.Disable(task);
      taskDatabase.Enable(task);
      taskDatabase.GetPendingTasks().Should().BeEmpty();
      taskDatabase.MarkDone(task);
      taskDatabase.Remove(task);
      taskDatabase.RemoveItemTasks(item);
      taskDatabase.RemoveItemTasks(item, typeof(Task));
      taskDatabase.Update(task, false);
      taskDatabase.UpdateItemTask(task, false);
    }

    [Fact]
    public void ShouldGetTheSamePendingTasks()
    {
      // arrange
      var taskDatabase = new StubTaskDatabase();

      // act & assert
      taskDatabase.GetPendingTasks().Should().BeSameAs(taskDatabase.GetPendingTasks());
    }
  }
}