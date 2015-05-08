namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using System;
  using FluentAssertions;
  using Xunit;

  [Trait("Deserialize", "Deserializing a new item")]
  public class DeserializeNewItem : IDisposable
  {
    private readonly Db db;

    public DeserializeNewItem()
    {
      this.db = new Db { new DsDbItem(SerializedItemIds.ContentHome) };
    }

    [Fact(DisplayName = "Creates the new item")]
    public void CreateNewItem()
    {
      this.db.GetItem(SerializedItemIds.ContentHome).Should().NotBeNull();
    }

    public void Dispose()
    {
      this.db.Dispose();
    }
  }
}