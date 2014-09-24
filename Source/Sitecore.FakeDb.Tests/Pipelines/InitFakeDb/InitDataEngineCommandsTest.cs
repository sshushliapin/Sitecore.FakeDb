namespace Sitecore.FakeDb.Tests.Pipelines.InitFakeDb
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Pipelines.InitFakeDb;
  using Xunit;

  public class InitDataEngineCommandsTest : IDisposable
  {
    private readonly Database database;

    private readonly DataStorage dataStorage;

    public InitDataEngineCommandsTest()
    {
      this.database = Database.GetDatabase("master");
      this.dataStorage = Substitute.For<DataStorage>(database);
    }

    [Fact]
    public void ShouldSetDataStorageForIRequireDataStorageCommands()
    {
      // arrange
      var commands = this.database.Engines.DataEngine.Commands;

      commands.AddFromTemplatePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, IDataEngineCommand>();
      commands.CopyItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.CopyItemCommand, IDataEngineCommand>();
      commands.CreateItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.CreateItemCommand, IDataEngineCommand>();
      commands.DeletePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.DeleteItemCommand, IDataEngineCommand>();
      commands.GetChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetChildrenCommand, IDataEngineCommand>();
      commands.GetItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetItemCommand, IDataEngineCommand>();
      commands.GetParentPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetParentCommand, IDataEngineCommand>();
      commands.GetRootItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetRootItemCommand, IDataEngineCommand>();
      commands.HasChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.HasChildrenCommand, IDataEngineCommand>();
      commands.MoveItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.MoveItemCommand, IDataEngineCommand>();
      commands.RemoveVersionPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.RemoveVersionCommand, IDataEngineCommand>();
      commands.ResolvePathPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.ResolvePathCommand, IDataEngineCommand>();
      commands.SaveItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.SaveItemCommand, IDataEngineCommand>();

      var args = new InitDbArgs(this.database, this.dataStorage);
      var processor = new InitDataEngineCommands();

      // act
      processor.Process(args);

      // assert
      ((IDataEngineCommand)commands.AddFromTemplatePrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.CopyItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.CreateItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.DeletePrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.GetChildrenPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.GetItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.GetParentPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.GetRootItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.HasChildrenPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.MoveItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.RemoveVersionPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.ResolvePathPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
      ((IDataEngineCommand)commands.SaveItemPrototype).Received().Initialize(Arg.Is<DataEngineCommand>(c => c.DataStorage == this.dataStorage));
    }

    [Fact]
    public void ShouldNotSetDataStorageIfNoIRequireDataStorageCommandFound()
    {
      // arrange
      var commands = this.database.Engines.DataEngine.Commands;

      commands.AddFromTemplatePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand>();
      commands.CopyItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.CopyItemCommand>();
      commands.CreateItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.CreateItemCommand>();
      commands.DeletePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.DeleteItemCommand>();
      commands.GetChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetChildrenCommand>();
      commands.GetItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetItemCommand>();
      commands.GetParentPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetParentCommand>();
      commands.GetRootItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetRootItemCommand>();
      commands.HasChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.HasChildrenCommand>();
      commands.MoveItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.MoveItemCommand>();
      commands.ResolvePathPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.ResolvePathCommand>();
      commands.SaveItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.SaveItemCommand>();

      var args = new InitDbArgs(this.database, this.dataStorage);
      var processor = new InitDataEngineCommands();

      // act
      Action action = () => processor.Process(args);

      // assert
      action.ShouldNotThrow();
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}