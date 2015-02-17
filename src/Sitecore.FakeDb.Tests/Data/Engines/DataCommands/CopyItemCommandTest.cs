namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;
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
      this.command.Initialize(this.dataStorage);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      this.command.CreateInstance().Should().BeOfType<CopyItemCommand>();
    }

    [Fact]
    public void ShouldCopyItem()
    {
      // arrange
      var item = ItemHelper.CreateInstance(this.database, "home", this.itemId, this.templateId, ID.Null, new FieldList());
      var copy = ItemHelper.CreateInstance(this.database);
      var destination = ItemHelper.CreateInstance(this.database);

      this.dataStorage.GetFakeItem(this.itemId).Returns(new DbItem("home"));
      this.dataStorage.GetFakeItem(this.copyId).Returns(new DbItem("copy"));
      this.dataStorage.GetSitecoreItem(this.copyId, Language.Current).Returns(copy);

      this.command.Initialize(item, destination, "copy of home", this.copyId, false);

      // act
      var result = this.command.DoExecute();

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