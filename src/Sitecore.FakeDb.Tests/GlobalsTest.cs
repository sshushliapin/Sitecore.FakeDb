namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.FakeDb.Links;
  using Sitecore.FakeDb.Tasks;
  using Xunit;

  public class GlobalsTest
  {
    [Fact]
    public void ShouldLoadLinkDatabase()
    {
      // act
      Globals.Load();

      // assert
      Globals.LinkDatabase.Should().BeOfType<FakeLinkDatabase>();
      Globals.TaskDatabase.Should().BeOfType<FakeTaskDatabase>();
    }

    [Fact]
    public void ShouldInitializeOnFakeDbInit()
    {
      // link and task databases are not configured with single instnace so will be recreated every time Globals.Load() runs
      // arrange
      Globals.Load();
      var linkDb = Globals.LinkDatabase;
      var taskDb = Globals.TaskDatabase;

      using (var db = new Db())
      {
        Globals.LinkDatabase.Should().NotBeNull();
        Globals.LinkDatabase.Should().BeOfType<FakeLinkDatabase>();
        Globals.LinkDatabase.Should().NotBe(linkDb);

        Globals.TaskDatabase.Should().NotBeNull();
        Globals.TaskDatabase.Should().BeOfType<FakeTaskDatabase>();
        Globals.TaskDatabase.Should().NotBe(taskDb);
      }
    }
  }
}