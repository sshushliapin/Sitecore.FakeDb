namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class AddFromTemplateCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenAddFromTemplateCommand();
      command.Initialize(this.dataStorage);

      // act & assert
      command.CreateInstance().Should().BeOfType<AddFromTemplateCommand>();
    }

    [Fact]
    public void ShouldCreateItem()
    {
      // arrange
      var itemId = ID.NewID;
      var templateId = ID.NewID;

      var item = ItemHelper.CreateInstance(this.database);
      var destination = ItemHelper.CreateInstance(this.database);

      this.dataStorage.GetSitecoreItem(itemId).Returns(item);

      var command = new OpenAddFromTemplateCommand { Engine = new DataEngine(database) };
      command.Initialize("home", templateId, destination, itemId);
      command.Initialize(this.dataStorage);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(item);
      this.dataStorage.Received().AddFakeItem(Arg.Is<DbItem>(i => i.Name == "home" && 
                                                                  i.ID == itemId &&
                                                                  i.TemplateID == templateId && 
                                                                  i.ParentID == destination.ID));
    }

    private class OpenAddFromTemplateCommand : AddFromTemplateCommand
    {
      public new Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand CreateInstance()
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