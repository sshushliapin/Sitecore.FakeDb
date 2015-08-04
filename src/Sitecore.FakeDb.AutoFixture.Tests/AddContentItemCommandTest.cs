namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class AddContentItemCommandTest
  {
    [Fact]
    public void ShouldAddItemToDatabase()
    {
      // arrange
      using (var db = new Db())
      {
        var sut = new AddContentItemCommand(db);
        var item = ItemHelper.CreateInstance();

        // act
        sut.Execute(item, null);

        // assert
        db.GetItem("/sitecore/content").Children.Should().HaveCount(1);
      }
    }
  }
}