namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class AddContentItemCommandTest
  {
    [Theory, AutoData]
    public void ExecuteAddsItemToDatabase([Frozen]Db db, AddContentItemCommand sut)
    {
      var item = ItemHelper.CreateInstance();

      sut.Execute(item, null);

      db.GetItem(item.ID).Should().NotBeNull();
    }

    [Theory, AutoData]
    public void AddsAddItemToContentRoot([Frozen]Db db, AddContentItemCommand sut)
    {
      var item = ItemHelper.CreateInstance();

      sut.Execute(item, null);

      db.GetItem("/sitecore/content").Children.Should().HaveCount(1);
    }

    [Theory, AutoData]
    public void ExecuteSetsItemTemplateId([Frozen]Db db, AddContentItemCommand sut)
    {
      var item = ItemHelper.CreateInstance();

      sut.Execute(item, null);

      db.GetItem(item.ID).TemplateID.Should().Be(item.TemplateID);
    }
  }
}