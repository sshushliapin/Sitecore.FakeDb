namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using System;
  using Xunit;

  [Trait("Deserialize", "Deserializing an item and adding a duplicate")]
  public class DeserializeItemAndAddDuplicate : IDisposable
  {
    private readonly Db db;

    private readonly DbItem duplicateItem;

    public DeserializeItemAndAddDuplicate()
    {
      this.db = new Db { new DsDbItem(SerializedItemIds.ContentHome) };
      this.duplicateItem = new DbItem("Home", SerializedItemIds.ContentHome);
    }

    [Fact(DisplayName = "Throws duplicate item exception")]
    public void ThrowException()
    {
      this.db.Add(this.duplicateItem);
    }

    [Fact(DisplayName = "Does not overwrite the deserialized item")]
    public void DoesNotOverwriteDeserializedItem()
    {
      this.db.Add(this.duplicateItem);
    }

    public void Dispose()
    {
      this.db.Dispose();
    }
  }
}