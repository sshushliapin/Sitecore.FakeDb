namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class RemoveVersionCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenRemoveVersionCommand();
      command.Initialize(this.dataStorage);

      // act & assert
      command.CreateInstance().Should().BeOfType<RemoveVersionCommand>();
    }

    [Fact]
    public void ShouldRemoveVersionFromFakeDbFields()
    {
      // arrange
      var itemId = ID.NewID;
      var dbitem = new DbItem("item") { Fields = { new DbField("Title") { { "en", "Hello!" } } } };
      this.dataStorage.GetFakeItem(itemId).Returns(dbitem);

      var item = ItemHelper.CreateInstance(this.database, itemId);

      var command = new OpenRemoveVersionCommand();
      command.Initialize(item);
      command.Initialize(this.dataStorage);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeTrue();
      dbitem.Fields.Single().Values["en"].Values.Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotRemoveVersionIfNoVersionFoundInSpecificLanguage()
    {
      // arrange
      var itemId = ID.NewID;
      var dbitem = new DbItem("item") { Fields = { new DbField("Title") } };
      this.dataStorage.GetFakeItem(itemId).Returns(dbitem);

      var item = ItemHelper.CreateInstance(this.database, itemId);

      var command = new OpenRemoveVersionCommand();
      command.Initialize(item);
      command.Initialize(this.dataStorage);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeFalse();
    }

    [Fact]
    public void ShouldDecreaseFakeItemVersionCount()
    {
      // arrange
      var itemId = ID.NewID;
      var dbitem = new DbItem("item");
      dbitem.VersionsCount.Add("en", 2);

      this.dataStorage.GetFakeItem(itemId).Returns(dbitem);

      var item = ItemHelper.CreateInstance(this.database, itemId);

      var command = new OpenRemoveVersionCommand();
      command.Initialize(item);
      command.Initialize(this.dataStorage);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeTrue();
      dbitem.VersionsCount["en"].Should().Be(1);
    }

    private class OpenRemoveVersionCommand : RemoveVersionCommand
    {
      public new Sitecore.Data.Engines.DataCommands.RemoveVersionCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new bool DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}