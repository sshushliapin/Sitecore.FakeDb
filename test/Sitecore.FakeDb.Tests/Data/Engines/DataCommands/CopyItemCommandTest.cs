namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;
  using Sitecore.Reflection;
  using Xunit;
  using CopyItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.CopyItemCommand;

  [Obsolete]
  public class CopyItemCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldCopyItem(CopyItemCommand sut, Item item, Item copy, Item destination)
    {
      // arrange
      sut.DataStorage.GetFakeItem(item.ID).Returns(new DbItem("home"));
      sut.DataStorage.GetFakeItem(copy.ID).Returns(new DbItem("copy"));
      sut.DataStorage.GetSitecoreItem(copy.ID, Language.Current).Returns(copy);

      sut.Initialize(item, destination, "copy of home", copy.ID, false);

      // act
      var result = ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      result.Should().Be(copy);
    }
  }
}