namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using System;
  using Xunit;

  [Trait("Deserialize", "Deserializing an item that is already exists")]
  public class DeserializeExistingItem : IDisposable
  {
    private readonly Db db;

    private readonly DsDbItem deserializedItem;

    public DeserializeExistingItem()
    {
      this.db = new Db { new DbItem("Home", SerializedItemIds.ContentHome) };
      this.deserializedItem = new DsDbItem(SerializedItemIds.ContentHome);
    }

    [Fact(DisplayName = "Does not throw an exception")]
    public void DoesNotThrow()
    {
      this.db.Add(this.deserializedItem);
    }

    [Fact(DisplayName = "Overwrites the existing item")]
    public void OverwriteExisting()
    {
      this.db.Add(this.deserializedItem);
    }

    public void Dispose()
    {
      this.db.Dispose();
    }
  }
}