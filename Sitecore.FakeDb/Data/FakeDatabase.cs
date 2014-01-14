namespace Sitecore.FakeDb.Data
{
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;

  public class FakeDatabase : Database
  {
    public FakeDatabase(string name)
      : base(name)
    {
      this.DataStorage = new DataStorage(this);
    }

    public DataStorage DataStorage { get; set; }
  }
}