namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Collections;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.Globalization;
  using Sitecore.Reflection;
  using Xunit;

  public class GetVersionsCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldGetEmptyVersionCollectionIfNoFakeItemFound(GetVersionsCommand sut, Item item, Language language, DbItem dbitem)
    {
      // arrange
      sut.Initialize(item, language);

      // act
      var versionCollection = (VersionCollection)ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      versionCollection.Should().BeEmpty();
    }

    [Theory, DefaultAutoData]
    public void ShouldGetItemVersionsForLanguage(GetVersionsCommand sut, Item item, Language language, DbItem versionedItem)
    {
      // arrange
      versionedItem.Fields.Add(new DbField("Title") { { "en", 1, "value1" }, { "en", 2, "value2" } });
      sut.DataStorage.GetFakeItem(item.ID).Returns(versionedItem);

      sut.Initialize(item, language);

      // act
      var versionCollection = (VersionCollection)ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      versionCollection.Count.Should().Be(2);
      versionCollection.Should().BeEquivalentTo(new Version(1), new Version(2));
    }

    [Theory, DefaultAutoData]
    public void ShouldGetItemVersionsCount(GetVersionsCommand sut, Item item, Language language, DbItem versionedItem)
    {
      // arrange
      versionedItem.AddVersion(language.Name);
      versionedItem.AddVersion(language.Name);
      sut.DataStorage.GetFakeItem(item.ID).Returns(versionedItem);

      sut.Initialize(item, language);

      // act
      var versionCollection = (VersionCollection)ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      versionCollection.Count.Should().Be(2);
      versionCollection.Should().BeEquivalentTo(new Version(1), new Version(2));
    }
  }
}