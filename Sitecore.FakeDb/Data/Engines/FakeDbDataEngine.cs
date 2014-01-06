namespace Sitecore.FakeDb.Data.Engines
{
  using Sitecore.Data;
  using Sitecore.Data.Engines;

  public class FakeDbDataEngine : DataEngine
  {
    public FakeDbDataEngine(Database database)
      : base(database)
    {
    }

    public FakeDbDataStorage Storage { get; set; }
  }
}