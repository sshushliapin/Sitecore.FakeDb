namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class DeleteItemCommandTest : CommandTestBase
  {
    private readonly OpenDeleteItemCommand command;

    public DeleteItemCommandTest()
    {
      this.command = new OpenDeleteItemCommand(this.dataStorage) { Engine = new DataEngine(this.database) };
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      this.command.CreateInstance().Should().BeOfType<DeleteItemCommand>();
    }

    [Fact]
    public void ShouldDeleteItemFromDataStorageAndReturnTrue()
    {
      // arrange
      var itemId = ID.NewID;

      this.dataStorage.FakeItems.Add(itemId, new DbItem("item"));
      this.command.Initialize(ItemHelper.CreateInstance(itemId, this.database), ID.NewID);

      // act
      var result = this.command.DoExecute();

      // assert
      result.Should().BeTrue();
      this.dataStorage.FakeItems.Should().NotContainKey(itemId);
    }

    [Fact]
    public void ShouldDeleteItemDescendants()
    {
      // arrange
      var itemId = ID.NewID;
      var desc1Id = ID.NewID;
      var desc2Id = ID.NewID;

      var item = new DbItem("item", itemId);
      var desc1 = new DbItem("descendant1", desc1Id);
      var desc2 = new DbItem("descendant2", desc2Id);

      item.Children.Add(desc1);
      desc1.Children.Add(desc2);

      this.dataStorage.FakeItems.Add(itemId, item);
      this.dataStorage.FakeItems.Add(desc1Id, desc1);
      this.dataStorage.FakeItems.Add(desc2Id, desc2);

      this.command.Initialize(ItemHelper.CreateInstance(itemId, this.database), ID.NewID);

      // act
      this.command.DoExecute();

      // assert
      this.dataStorage.FakeItems.Should().NotContainKey(desc1Id);
      this.dataStorage.FakeItems.Should().NotContainKey(desc2Id);
    }

    [Fact]
    public void ShouldReturnFalseIfNoItemFound()
    {
      // arrange
      this.command.Initialize(ItemHelper.CreateInstance(this.database), ID.NewID);

      // act
      var result = this.command.DoExecute();

      // assert
      result.Should().BeFalse();
    }

    private class OpenDeleteItemCommand : DeleteItemCommand
    {
      public OpenDeleteItemCommand(DataStorage dataStorage)
        : base(dataStorage)
      {
      }

      public new Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
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