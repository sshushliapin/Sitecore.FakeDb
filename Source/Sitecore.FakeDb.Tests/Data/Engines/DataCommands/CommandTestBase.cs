namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using NSubstitute;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;

  public abstract class CommandTestBase
  {
    protected readonly FakeDatabase database;

    protected readonly DataStorage dataStorage;

    protected CommandTestBase()
    {
      this.database = Substitute.For<FakeDatabase>("master");
      this.dataStorage = Substitute.For<DataStorage>();
    }
  }
}