namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Xunit;

  public class AddContentDbItemCommandTest
  {
    [Fact]
    public void ShouldAddDbItemToDatabase()
    {
      // arrange
      using (var db = new Db())
      {
        var sut = new AddContentDbItemCommand(db);
        var item = new DbItem("home");

        // act
        sut.Execute(item, null);

        // assert
        db.GetItem("/sitecore/content").Children.Should().HaveCount(1);
      }
    }
  }
}