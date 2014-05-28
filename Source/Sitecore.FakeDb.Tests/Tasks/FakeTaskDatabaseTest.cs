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

  public class FakeTaskDatabaseTest
  {
    [Fact]
    public void ShouldBeQuiet()
    {
      // arrange
      var tasksDatabase = new FakeTaskDatabase();
      var task = Substitute.For<Task>(DateTime.Now);
      var item = ItemHelper.CreateInstance(Database.GetDatabase("master"));

      // act & assert
      tasksDatabase.Disable(task);
      tasksDatabase.Enable(task);
      tasksDatabase.GetPendingTasks().Should().BeEmpty();
      tasksDatabase.MarkDone(task);
      tasksDatabase.Remove(task);
      tasksDatabase.RemoveItemTasks(item);
      tasksDatabase.RemoveItemTasks(item, typeof(Task));
      tasksDatabase.Update(task, false);
      tasksDatabase.UpdateItemTask(task, false);
    }
  }
}