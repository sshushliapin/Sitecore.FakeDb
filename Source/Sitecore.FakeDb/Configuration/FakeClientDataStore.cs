namespace Sitecore.FakeDb.Configuration
{
  using System;
  using Sitecore.Configuration;

  public class FakeClientDataStore : ClientDataStore
  {
    public FakeClientDataStore()
      : base(new TimeSpan())
    {
    }

    protected override void CompactData()
    {
      throw new NotImplementedException();
    }

    protected override string LoadData(string key)
    {
      throw new NotImplementedException();
    }

    protected override void SaveData(string key, string data)
    {
      throw new NotImplementedException();
    }

    protected override void RemoveData(string key)
    {
      throw new NotImplementedException();
    }
  }
}