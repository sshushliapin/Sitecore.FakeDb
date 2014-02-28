namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;

  public abstract class CommandTestBase
  {
    protected readonly Database database;

    protected readonly DataStorage dataStorage;

    protected CommandTestBase()
    {
      this.database = Database.GetDatabase("master");
      this.dataStorage = Substitute.For<DataStorage>();
    }
  }
}