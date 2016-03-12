namespace Sitecore.FakeDb.Tests.Pipelines.AddDbItem
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines.AddDbItem;
  using Xunit;

  public class AddVersionTest
  {
    [Fact]
    public void ShouldAddFirstVersion()
    {
      // arrange
      var processor = new AddVersion();
      var args = new AddDbItemArgs(new DbItem("home"), new DataStorage(Database.GetDatabase("master")));

      // act
      processor.Process(args);

      // assert
      args.DbItem.GetVersionCount("en").Should().Be(1);
    }
  }
}