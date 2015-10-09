namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;
  using Sitecore.Reflection;
  using Xunit;

  public class AddVersionCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldAddVersionToFakeDbFieldsUsingItemLanguage(AddVersionCommand sut, Item item)
    {
      // arrange
      var dbitem = new DbItem("item") { Fields = { new DbField("Title") { { "en", "Hello!" }, { "da", "Hej!" } } } };
      sut.DataStorage.GetFakeItem(item.ID).Returns(dbitem);

      sut.Initialize(item);

      // act
      ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      dbitem.Fields.Single().Values["en"][1].Should().Be("Hello!");
      dbitem.Fields.Single().Values["en"][2].Should().Be("Hello!");
      dbitem.Fields.Single().Values["da"][1].Should().Be("Hej!");
      dbitem.Fields.Single().Values["da"].ContainsKey(2).Should().BeFalse();
    }

    [Theory, DefaultAutoData]
    public void ShouldGetNewItemVersion(AddVersionCommand sut, ID itemId)
    {
      // arrange
      var dbitem = new DbItem("home") { { "Title", "Hello!" } };
      sut.DataStorage.GetFakeItem(itemId).Returns(dbitem);

      var originalItem = ItemHelper.CreateInstance(itemId);
      var itemWithNewVersion = ItemHelper.CreateInstance(itemId);
      sut.DataStorage.GetSitecoreItem(itemId, Language.Parse("en"), Version.Parse(2)).Returns(itemWithNewVersion);

      sut.Initialize(originalItem);

      // act
      var result = ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      result.Should().BeSameAs(itemWithNewVersion);
    }

    [Theory, DefaultAutoData]
    public void ShouldAddVersionIfNoVersionExistsInSpecificLanguage(AddVersionCommand sut, Item item)
    {
      // arrange
      var dbitem = new DbItem("item") { Fields = { new DbField("Title") } };
      sut.DataStorage.GetFakeItem(item.ID).Returns(dbitem);

      sut.Initialize(item);

      // act
      ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      dbitem.Fields.Single().Values["en"][1].Should().BeEmpty();
    }

    [Theory, DefaultAutoData]
    public void ShouldIncreaseFakeItemVersionCount(AddVersionCommand sut, Item item, DbItem dbItem)
    {
      // arrange
      sut.DataStorage.GetFakeItem(item.ID).Returns(dbItem);
      sut.Initialize(item);

      // act
      ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      dbItem.GetVersionCount("en").Should().Be(2);
    }
  }
}