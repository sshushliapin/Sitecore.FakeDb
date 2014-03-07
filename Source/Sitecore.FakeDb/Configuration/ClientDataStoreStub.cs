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