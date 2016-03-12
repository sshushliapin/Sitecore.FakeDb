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

  public class FakeTaskDatabaseTest : IDisposable
  {
    private readonly FakeTaskDatabase taskDatabase;

    private readonly TaskDatabase behavior;

    private readonly Task task;

    public FakeTaskDatabaseTest()
    {
      this.task = Substitute.For<Task>(DateTime.Now);
      this.behavior = Substitute.For<TaskDatabase>();
      this.taskDatabase = new FakeTaskDatabase();
      this.taskDatabase.LocalProvider.Value = this.behavior;
    }

    [Fact]
    public void ShouldBeQuiet()
    {
      // arrange
      var stubTaskDatabase = new FakeTaskDatabase();
      var item = ItemHelper.CreateInstance(Database.GetDatabase("master"));

      // act & assert
      stubTaskDatabase.Disable(this.task);
      stubTaskDatabase.Enable(this.task);
      stubTaskDatabase.GetPendingTasks().Should().BeEmpty();
      stubTaskDatabase.MarkDone(this.task);
      stubTaskDatabase.Remove(this.task);
      stubTaskDatabase.RemoveItemTasks(item);
      stubTaskDatabase.RemoveItemTasks(item, typeof(Task));
      stubTaskDatabase.Update(this.task, false);
      stubTaskDatabase.UpdateItemTask(this.task, false);
    }

    [Fact]
    public void ShouldGetTheSamePendingTasks()
    {
      // arrange
      var stubTaskDatabase = new FakeTaskDatabase();

      // act & assert
      stubTaskDatabase.GetPendingTasks().Should().BeSameAs(stubTaskDatabase.GetPendingTasks());
    }

    [Fact]
    public void ShouldCallDisable()
    {
      // act
      this.taskDatabase.Disable(this.task);

      // assert
      this.behavior.Received().Disable(this.task);
    }

    [Fact]
    public void ShouldCallEnable()
    {
      // act
      this.taskDatabase.Enable(this.task);

      // assert
      this.behavior.Received().Enable(this.task);
    }

    [Fact]
    public void ShouldCallGetPendingTasks()
    {
      // act
      this.taskDatabase.GetPendingTasks();

      // assert
      this.behavior.Received().GetPendingTasks();
    }

    [Fact]
    public void ShouldCallMarkDone()
    {
      // act
      this.taskDatabase.MarkDone(this.task);

      // assert
      this.behavior.Received().MarkDone(this.task);
    }

    [Fact]
    public void ShouldCallRemove()
    {
      // act
      this.taskDatabase.Remove(this.task);

      // assert
      this.behavior.Received().Remove(this.task);
    }

    [Fact]
    public void ShouldCallRemoveItemTask()
    {
      // act
      var item = ItemHelper.CreateInstance();
      this.taskDatabase.RemoveItemTasks(item);

      // assert
      this.behavior.Received().RemoveItemTasks(item);
    }

    [Fact]
    public void ShouldCallRemoveItemTaskWithType()
    {
      // act
      var item = ItemHelper.CreateInstance();
      this.taskDatabase.RemoveItemTasks(item, typeof(Task));

      // assert
      this.behavior.Received().RemoveItemTasks(item, typeof(Task));
    }

    [Fact]
    public void ShouldCallUpdate()
    {
      // act
      this.taskDatabase.Update(this.task, true);

      // assert
      this.behavior.Received().Update(this.task, true);
    }

    [Fact]
    public void ShouldCallUpdateItemTask()
    {
      // act
      this.taskDatabase.UpdateItemTask(this.task, true);

      // assert
      this.behavior.Received().UpdateItemTask(this.task, true);
    }

    public void Dispose()
    {
      this.taskDatabase.Dispose();
    }
  }
}