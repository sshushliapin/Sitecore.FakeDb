namespace Sitecore.FakeDb.Tests.Pipelines.InitFakeDb
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines.InitFakeDb;
  using System;
  using Xunit;

  public class InitDataEngineCommandsTest
  {
    [Fact]
    public void ShouldSetDataStorageForIRequireDataStorageCommands()
    {
      // arrange
      var database = Database.GetDatabase("master");
      var commands = database.Engines.DataEngine.Commands;

      commands.AddFromTemplatePrototype = Substitute.For<AddFromTemplateCommand, IRequireDataStorage>();
      commands.CreateItemPrototype = Substitute.For<CreateItemCommand, IRequireDataStorage>();
      commands.DeletePrototype = Substitute.For<DeleteItemCommand, IRequireDataStorage>();
      commands.GetChildrenPrototype = Substitute.For<GetChildrenCommand, IRequireDataStorage>();
      commands.GetItemPrototype = Substitute.For<GetItemCommand, IRequireDataStorage>();
      commands.GetParentPrototype = Substitute.For<GetParentCommand, IRequireDataStorage>();
      commands.GetRootItemPrototype = Substitute.For<GetRootItemCommand, IRequireDataStorage>();
      commands.HasChildrenPrototype = Substitute.For<HasChildrenCommand, IRequireDataStorage>();
      commands.ResolvePathPrototype = Substitute.For<ResolvePathCommand, IRequireDataStorage>();
      commands.SaveItemPrototype = Substitute.For<SaveItemCommand, IRequireDataStorage>();

      var dataStorage = Substitute.For<DataStorage>();

      var args = new InitDbArgs(database, dataStorage);
      var processor = new InitDataEngineCommands();

      // act
      processor.Process(args);

      // assert
      ((IRequireDataStorage)commands.AddFromTemplatePrototype).Received().SetDataStorage(dataStorage);
      ((IRequireDataStorage)commands.CreateItemPrototype).Received().SetDataStorage(dataStorage);
      ((IRequireDataStorage)commands.DeletePrototype).Received().SetDataStorage(dataStorage);
      ((IRequireDataStorage)commands.GetChildrenPrototype).Received().SetDataStorage(dataStorage);
      ((IRequireDataStorage)commands.GetItemPrototype).Received().SetDataStorage(dataStorage);
      ((IRequireDataStorage)commands.GetParentPrototype).Received().SetDataStorage(dataStorage);
      ((IRequireDataStorage)commands.GetRootItemPrototype).Received().SetDataStorage(dataStorage);
      ((IRequireDataStorage)commands.HasChildrenPrototype).Received().SetDataStorage(dataStorage);
      ((IRequireDataStorage)commands.ResolvePathPrototype).Received().SetDataStorage(dataStorage);
      ((IRequireDataStorage)commands.SaveItemPrototype).Received().SetDataStorage(dataStorage);
    }

    [Fact]
    public void ShouldNotSetDataStorageIfNoIRequireDataStorageCommandFound()
    {
      // arrange
      var database = Database.GetDatabase("master");
      var commands = database.Engines.DataEngine.Commands;

      commands.AddFromTemplatePrototype = Substitute.For<AddFromTemplateCommand>();
      commands.CreateItemPrototype = Substitute.For<CreateItemCommand>();
      commands.DeletePrototype = Substitute.For<DeleteItemCommand>();
      commands.GetChildrenPrototype = Substitute.For<GetChildrenCommand>();
      commands.GetItemPrototype = Substitute.For<GetItemCommand>();
      commands.GetParentPrototype = Substitute.For<GetParentCommand>();
      commands.GetRootItemPrototype = Substitute.For<GetRootItemCommand>();
      commands.HasChildrenPrototype = Substitute.For<HasChildrenCommand>();
      commands.ResolvePathPrototype = Substitute.For<ResolvePathCommand>();
      commands.SaveItemPrototype = Substitute.For<SaveItemCommand>();

      var args = new InitDbArgs(database, Substitute.For<DataStorage>());
      var processor = new InitDataEngineCommands();

      // act
      Action action = () => processor.Process(args);

      // assert
      action.ShouldNotThrow();
    }
  }
}
