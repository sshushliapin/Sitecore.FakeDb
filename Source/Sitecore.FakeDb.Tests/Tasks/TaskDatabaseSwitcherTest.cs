namespace Sitecore.FakeDb.Tests.Tasks
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Tasks;
  using Sitecore.Tasks;
  using Xunit;

  public class TaskDatabaseSwitcherTest
  {
    [Fact]
    public void ShouldSwitchTaskDatabaseBehavior()
    {
      // arrange
      Globals.Load();

      var behavior = Substitute.For<TaskDatabase>();

      // act & assert
      using (new TaskDatabaseSwitcher(behavior))
      {
        ((FakeTaskDatabase)Globals.TaskDatabase).Behavior.Should().Be(behavior);
      }
    }
  }
}