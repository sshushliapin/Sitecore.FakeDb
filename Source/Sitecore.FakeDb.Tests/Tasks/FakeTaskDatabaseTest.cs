namespace Sitecore.FakeDb.Tests.Tasks
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Tasks;
  using Sitecore.Tasks;
  using Xunit;

  public class FakeTaskDatabaseTest
  {
    private readonly FakeTaskDatabase taskDatabase;

    private readonly TaskDatabase behavior;

    private readonly Task task;

    public FakeTaskDatabaseTest()
    {
      task = Substitute.For<Task>(DateTime.Now);
      behavior = Substitute.For<TaskDatabase>();
      taskDatabase = new FakeTaskDatabase { Behavior = this.behavior };
    }

    [Fact]
    public void ShouldGetStubTaskDatabaseBehaviorByDefault()
    {
      // act & assert
      new FakeTaskDatabase().Behavior.Should().BeOfType<StubTaskDatabase>();
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
  }
}