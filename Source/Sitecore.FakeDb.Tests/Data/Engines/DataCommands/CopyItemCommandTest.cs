namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;
  using CopyItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.CopyItemCommand;

  public class CopyItemCommandTest : CommandTestBase
  {
    private readonly ID templateId;

    private readonly ID itemId;

    private readonly ID copyId;

    private readonly OpenCopyItemCommand command;

    public CopyItemCommandTest()
    {
      this.templateId = ID.NewID;
      this.itemId = ID.NewID;
      this.copyId = ID.NewID;

      this.command = new OpenCopyItemCommand { Engine = new DataEngine(this.database) };
      this.command.Initialize(this.innerCommand);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var createdCommand = Substitute.For<CopyItemCommand>();
      this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.CopyItemCommand, CopyItemCommand>().Returns(createdCommand);

      // act & assert
      this.command.CreateInstance().Should().Be(createdCommand);
    }

    [Fact]
    public void ShouldCreateDefaultCreator()
    {
      // act & assert
      this.command.ItemCreator.Should().NotBeNull();
      this.command.ItemCreator.DataStorage.Should().Be(this.dataStorage);
    }

    [Fact]
    public void ShouldCopyItem()
    {
      // arrange
      var item = ItemHelper.CreateInstance("home", itemId, templateId, new FieldList(), this.database);
      var copy = ItemHelper.CreateInstance(this.database);
      var destination = ItemHelper.CreateInstance(this.database);

      this.command.ItemCreator = Substitute.For<ItemCreator>(this.dataStorage);
      this.command.ItemCreator.Create("copy of home", this.copyId, templateId, database, destination).Returns(copy);

      this.command.Initialize(item, destination, "copy of home", this.copyId, false);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(copy);
    }

    private class OpenCopyItemCommand : CopyItemCommand
    {
      public new Sitecore.Data.Engines.DataCommands.CopyItemCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new Item DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}