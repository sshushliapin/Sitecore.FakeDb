namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using FluentAssertions;
  using Xunit;

  [Trait("Deserialize", "Deserializing an item that is already exists")]
  public class DeserializeExistingItem : DeserializeTestBase
  {
    public DeserializeExistingItem()
    {
      this.Db.Add(this.AdhocItem);
    }

    [Fact(DisplayName = "Does not throw an exception")]
    public void DoesNotThrow()
    {
      this.Db.Add(this.DeserializedItem);
    }

    [Fact(DisplayName = "Overwrites the existing item")]
    public void OverwriteExisting()
    {
      this.Db.Add(this.DeserializedItem);
      this.Db.GetItem(SerializationId.ContentHomeItem)["Title"].Should().Be("Sitecore");
    }
  }
}