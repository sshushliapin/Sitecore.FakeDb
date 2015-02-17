namespace Sitecore.FakeDb
{
  using Sitecore.FakeDb.Data.Engines;

  public interface IRequireDataStorage
  {
    DataStorage DataStorage { get; }

    void SetDataStorage(DataStorage dataStorage);
  }
}