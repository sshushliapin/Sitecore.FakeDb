namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using NSubstitute;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;

  public abstract class CommandTestBase : IDisposable
  {
    protected readonly Database database;

    protected readonly DataStorage dataStorage;

    protected readonly DataEngineCommand innerCommand;

    protected CommandTestBase()
    {
      this.database = Database.GetDatabase("master");
      this.dataStorage = Substitute.For<DataStorage>(this.database);

      this.innerCommand = Substitute.For<DataEngineCommand>(this.dataStorage);
      this.innerCommand.DataStorage.Returns(this.dataStorage);
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}