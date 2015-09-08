namespace Sitecore.FakeDb.Data.Engines
{
  using Sitecore.Common;

  public class DataStorageSwitcher : Switcher<DataStorage>
  {
    public DataStorageSwitcher(DataStorage objectToSwitchTo)
      : base(objectToSwitchTo)
    {
    }
  }
}