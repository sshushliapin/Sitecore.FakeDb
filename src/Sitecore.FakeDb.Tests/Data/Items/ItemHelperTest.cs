namespace Sitecore.FakeDb.Tests.Data.Items
{
  using System;
  using FluentAssertions;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;
  using Xunit;

  public class ItemHelperTest : IDisposable
  {
    private const string Name = "home";

    [Fact]
    public void ShouldSimpleCreateItem()
    {
      // arrange
      var database = Database.GetDatabase("master");
      var item = ItemHelper.CreateInstance(database, Name);

      // assert
      item.Name.Should().Be(Name);
      item.ID.Should().NotBeNull();
      item.TemplateID.Should().NotBeNull();
      item.Database.Should().NotBeNull();
      item.Language.Should().Be(Language.Parse("en"));
    }

    [Fact]
    public void ShouldCreateItem()
    {
      var id = ID.NewID;
      var templateId = ID.NewID;
      var database = Database.GetDatabase("master");
      var language = Language.Parse("uk-UA");

      // arrange
      var item = ItemHelper.CreateInstance(database, Name, id, templateId, ID.NewID, new FieldList(), language);

      // assert
      item.Name.Should().Be(Name, "name");
      item.ID.Should().Be(id, "id");
      item.TemplateID.Should().Be(templateId, "templateId");
      item.Database.Should().Be(database, "database");
      item.Language.Should().Be(language, "language");
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}