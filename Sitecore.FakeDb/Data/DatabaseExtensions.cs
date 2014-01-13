namespace Sitecore.FakeDb.Data
{
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;

  public static class DatabaseExtensions
  {
    public static DataStorage GetDataStorage(this Database database)
    {
      return ((FakeDatabase)database).DataStorage;
    }
  }
}