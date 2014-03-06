namespace Sitecore.FakeDb.Tests.Pipelines.InitFakeDb
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Pipelines.InitFakeDb;
  using Xunit;

  public class InitDataEngineCommandsTest
  {
    [Fact]
    public void ShouldSetDataStorageForIRequireDataStorageCommands()
    {
      // arrange
      var database = Database.GetDatabase("master");
      var commands = database.Engines.DataEngine.Commands;

      commands.AddFromTemplatePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, IDataEngineCommand>();
      commands.CreateItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.CreateItemCommand, IDataEngineCommand>();
      commands.DeletePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.DeleteItemCommand, IDataEngineCommand>();
      commands.GetChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetChildrenCommand, IDataEngineCommand>();
      commands.GetItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetItemCommand, IDataEngineCommand>();
      commands.GetParentPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetParentCommand, IDataEngineCommand>();
      commands.GetRootItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetRootItemCommand, IDataEngineCommand>();
      commands.HasChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.HasChildrenCommand, IDataEngineCommand>();
      commands.ResolvePathPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.ResolvePathCommand, IDataEngineCommand>();
      commands.SaveItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.SaveItemCommand, IDataEngineCommand>();

      var dataStorage = Substitute.For<DataStorage>();

      var args = new InitDbArgs(database, dataStorage);
      var processor = new InitDataEngineCommands();

      // act
      processor.Process(args);

      // assert
      ((IDataEngineCommand)commands.AddFromTemplatePrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
      ((IDataEngineCommand)commands.CreateItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
      ((IDataEngineCommand)commands.DeletePrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
      ((IDataEngineCommand)commands.GetChildrenPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
      ((IDataEngineCommand)commands.GetItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
      ((IDataEngineCommand)commands.GetParentPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
      ((IDataEngineCommand)commands.GetRootItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
      ((IDataEngineCommand)commands.HasChildrenPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
      ((IDataEngineCommand)commands.ResolvePathPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
      ((IDataEngineCommand)commands.SaveItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == dataStorage));
    }

    [Fact]
    public void ShouldNotSetDataStorageIfNoIRequireDataStorageCommandFound()
    {
      // arrange
      var database = Database.GetDatabase("master");
      var commands = database.Engines.DataEngine.Commands;

      commands.AddFromTemplatePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand>();
      commands.CreateItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.CreateItemCommand>();
      commands.DeletePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.DeleteItemCommand>();
      commands.GetChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetChildrenCommand>();
      commands.GetItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetItemCommand>();
      commands.GetParentPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetParentCommand>();
      commands.GetRootItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetRootItemCommand>();
      commands.HasChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.HasChildrenCommand>();
      commands.ResolvePathPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.ResolvePathCommand>();
      commands.SaveItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.SaveItemCommand>();

      var args = new InitDbArgs(database, Substitute.For<DataStorage>());
      var processor = new InitDataEngineCommands();

      // act
      Action action = () => processor.Process(args);

      // assert
      action.ShouldNotThrow();
    }
  }
}