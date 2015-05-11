namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using System;
  using FluentAssertions;
  using Xunit;

  [Trait("Deserialize", "Deserializing an item and adding a duplicate")]
  public class DeserializeItemAndAddDuplicate : DeserializeTestBase
  {

    public DeserializeItemAndAddDuplicate()
    {
      this.Db.Add(this.DeserializedItem);
    }

    [Fact(DisplayName = "Throws duplicate item exception")]
    public void ThrowException()
    {
      Assert.Throws<InvalidOperationException>(() => this.Db.Add(this.AdhocItem));
    }

    [Fact(DisplayName = "Does not overwrite the deserialized item")]
    public void DoesNotOverwriteDeserializedItem()
    {
      try
      {
        this.Db.Add(this.AdhocItem);
      }
      catch (InvalidOperationException)
      {
      }

      this.Db.GetItem(SerializationId.ContentHomeItem)["Title"].Should().Be("Sitecore");
    }
  }
}