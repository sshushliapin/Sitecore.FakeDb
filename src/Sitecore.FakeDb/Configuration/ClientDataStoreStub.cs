namespace Sitecore.FakeDb.Configuration
{
  using System;
  using Sitecore.Configuration;

  public class ClientDataStoreStub : ClientDataStore
  {
    public ClientDataStoreStub()
      : base(new TimeSpan(), false)
    {
    }

    protected override void CompactData()
    {
    }

    protected override string LoadData(string key)
    {
      return null;
    }

    protected override void SaveData(string key, string data)
    {
    }

    protected override void RemoveData(string key)
    {
    }
  }
}