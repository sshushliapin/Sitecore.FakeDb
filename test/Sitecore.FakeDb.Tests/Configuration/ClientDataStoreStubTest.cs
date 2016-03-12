namespace Sitecore.FakeDb.Tests.Configuration
{
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
    public void ShouldNotThrowExceptionOnCompactData()
    {
      // act & assert
      this.dataStore.CompactData();
    }

    [Fact]
    public void ShouldReturnNullOnLoadData()
    {
      // act & assert
      this.dataStore.LoadData("key").Should().BeNull();
    }

    [Fact]
    public void ShouldNotThrowExceptionOnSaveData()
    {
      // act & assert
      this.dataStore.SaveData("key", "data");
    }

    [Fact]
    public void ShouldNotThrowExceptionOnRemoveData()
    {
      // act & assert
      this.dataStore.RemoveData("key");
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