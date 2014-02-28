namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using NSubstitute;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;

  public abstract class CommandTestBase : IDisposable
  {
    protected readonly Database database;

    protected readonly DataStorage dataStorage;

    protected CommandTestBase()
    {
      this.database = Database.GetDatabase("master");
      this.dataStorage = Substitute.For<DataStorage>();
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}