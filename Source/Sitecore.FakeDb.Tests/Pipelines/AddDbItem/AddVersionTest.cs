namespace Sitecore.FakeDb.Tests.Pipelines.AddDbItem
{
  using FluentAssertions;
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
      var args = new AddDbItemArgs(new DbItem("home"), new DataStorage());

      // act
      processor.Process(args);

      // assert
      args.DbItem.VersionsCount["en"].Should().Be(1);
    }
  }
}