namespace Sitecore.FakeDb.Tests.Data.Items
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class ItemHelperTest
  {
    private const string Name = "home";

    [Fact]
    public void ShouldSimpleCreateItem()
    {
      // arrange
      var item = ItemHelper.CreateInstance(Name);

      // assert
      item.Name.Should().Be(Name);
      item.ID.Should().NotBeNull();
      item.TemplateID.Should().NotBeNull();
      item.Database.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateItem()
    {
      var id = ID.NewID;
      var templateId = ID.NewID;
      var database = Database.GetDatabase("master");

      // arrange
      var item = ItemHelper.CreateInstance(Name, id, templateId, new FieldList(), database);

      // assert
      item.Name.Should().Be(Name);
      item.ID.Should().Be(id);
      item.TemplateID.Should().Be(templateId);
      item.Database.Should().Be(database);
    }
  }
}