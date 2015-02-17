namespace Sitecore.FakeDb.Tests.Pipelines.InitFakeDb
{
  using System;
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
      this.dataStorage = Substitute.For<DataStorage>(this.database);
    }

    [Fact]
    public void ShouldSetDataStorageForIRequireDataStorageCommands()
    {
      // arrange
      var commands = this.database.Engines.DataEngine.Commands;

      commands.AddFromTemplatePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, IDataEngineCommand>();
      commands.AddVersionPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.AddVersionCommand, IDataEngineCommand>();
      commands.BlobStreamExistsPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand, IDataEngineCommand>();
      commands.CopyItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.CopyItemCommand, IDataEngineCommand>();
      commands.CreateItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.CreateItemCommand, IDataEngineCommand>();
      commands.DeletePrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.DeleteItemCommand, IDataEngineCommand>();
      commands.GetBlobStreamPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand, IDataEngineCommand>();
      commands.GetChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetChildrenCommand, IDataEngineCommand>();
      commands.GetItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetItemCommand, IDataEngineCommand>();
      commands.GetParentPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetParentCommand, IDataEngineCommand>();
      commands.GetRootItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetRootItemCommand, IDataEngineCommand>();
      commands.GetVersionsPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.GetVersionsCommand, IDataEngineCommand>();
      commands.HasChildrenPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.HasChildrenCommand, IDataEngineCommand>();
      commands.MoveItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.MoveItemCommand, IDataEngineCommand>();
      commands.RemoveVersionPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.RemoveVersionCommand, IDataEngineCommand>();
      commands.ResolvePathPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.ResolvePathCommand, IDataEngineCommand>();
      commands.SaveItemPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.SaveItemCommand, IDataEngineCommand>();
      commands.SetBlobStreamPrototype = Substitute.For<Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand, IDataEngineCommand>();

      var args = new InitDbArgs(this.database, this.dataStorage);
      var processor = new InitDataEngineCommands();

      // act
      processor.Process(args);

      // assert
      ((IDataEngineCommand)commands.AddFromTemplatePrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.AddVersionPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.BlobStreamExistsPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.CopyItemPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.CreateItemPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.DeletePrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.GetBlobStreamPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.GetChildrenPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.GetItemPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.GetParentPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.GetRootItemPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.GetVersionsPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.HasChildrenPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.MoveItemPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.RemoveVersionPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.ResolvePathPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.SaveItemPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
      ((IDataEngineCommand)commands.SetBlobStreamPrototype).Received().Initialize(Arg.Is<DataStorage>(ds => ds == this.dataStorage));
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}