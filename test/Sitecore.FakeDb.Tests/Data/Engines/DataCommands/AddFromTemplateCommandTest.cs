namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.Reflection;
  using Sitecore.Globalization;
  using Xunit;

  [Obsolete]
  public class AddFromTemplateCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldAddFakeItem(AddFromTemplateCommand sut, string name, ID templateId, Item destination, ID newId)
    {
      // arrange
      sut.Initialize(name, templateId, destination, newId);

      // act
      ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      sut.DataStorage.Received().AddFakeItem(Arg.Is<DbItem>(i => i.Name == name &&
                                                                 i.ID == newId &&
                                                                 i.TemplateID == templateId &&
                                                                 i.ParentID == destination.ID),
                                             Arg.Is<Language>(l => l.Name == Language.Current.Name));
    }

    [Theory, DefaultAutoData]
    public void ShouldReturnCreatedItem(AddFromTemplateCommand sut, Item item, Item destination)
    {
      // arrange
      sut.DataStorage.GetSitecoreItem(item.ID, item.Language).Returns(item);
      sut.Initialize(item.Name, item.TemplateID, destination, item.ID);

      // act
      var result = ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      result.Should().Be(item);
    }
  }
}