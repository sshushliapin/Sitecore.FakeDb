namespace Sitecore.FakeDb.Tests.Configuration
{
  using System;
  using FluentAssertions;
  using Sitecore.FakeDb.Configuration;
  using Xunit;

  public class ClientDataStoreStubTest
  {
    private readonly OpenClientDataStoreStub dataStore;

    public ClientDataStoreStubTest()
    {
      this.dataStore = new OpenClientDataStoreStub();
    }

    [Fact]
    public void ShouldThrowNotImplementedExceptionOnCompactData()
    {
      // act & assert
      Assert.Throws<NotImplementedException>(() => this.dataStore.CompactData());
    }

    [Fact]
    public void ShouldThrowNotImplementedExceptionOnLoadData()
    {
      // act & assert
      Assert.Throws<NotImplementedException>(() => this.dataStore.LoadData("key"));
    }

    [Fact]
    public void ShouldThrowNotImplementedExceptionOnSaveData()
    {
      // act & assert
      Assert.Throws<NotImplementedException>(() => this.dataStore.SaveData("key", "data"));
    }

    [Fact]
    public void ShouldThrowNotImplementedExceptionOnRemoveData()
    {
      // act & assert
      Assert.Throws<NotImplementedException>(() => this.dataStore.RemoveData("key"));
    }

    private class OpenClientDataStoreStub : ClientDataStoreStub
    {
      new public void CompactData()
      {
        base.CompactData();
      }

      new public string LoadData(string key)
      {
        return base.LoadData(key);
      }

      new public void SaveData(string key, string data)
      {
        base.SaveData(key, data);
      }

      new public void RemoveData(string key)
      {
        base.RemoveData(key);
      }
    }
  }
}