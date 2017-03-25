namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.Reflection;
  using Xunit;

  [Obsolete]
  public class CreateItemCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldAddFakeItem(CreateItemCommand sut, string name, ID templateId, Item destination, ID newId)
    {
      // arrange
      sut.Initialize(newId, name, templateId, destination);

      // act
      ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      sut.DataStorage.Received().AddFakeItem(Arg.Is<DbItem>(i => i.Name == name &&
                                                                 i.ID == newId &&
                                                                 i.TemplateID == templateId &&
                                                                 i.ParentID == destination.ID));
    }

    [Theory, DefaultAutoData]
    public void ShouldReturnCreatedItem(CreateItemCommand sut, Item item, Item destination)
    {
      // arrange
      sut.DataStorage.GetSitecoreItem(item.ID).Returns(item);
      sut.Initialize(item.ID, item.Name, item.TemplateID, destination);

      // act
      var result = ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      result.Should().Be(item);
    }
  }
}