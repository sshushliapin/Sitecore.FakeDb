namespace Sitecore.FakeDb.Data
{
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;

  public class FakeDatabase : Database
  {
    private DataStorage dataStorage = new DataStorage();

    public FakeDatabase(string name)
      : base(name)
    {
      dataStorage = new DataStorage();
      dataStorage.SetDatabase(this);
    }

    public DataStorage DataStorage
    {
      get { return dataStorage; }
      set { dataStorage = value; }
    }
  }
}