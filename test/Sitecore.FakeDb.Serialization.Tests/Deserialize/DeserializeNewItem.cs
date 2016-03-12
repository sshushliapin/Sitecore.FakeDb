namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using FluentAssertions;
  using Xunit;

  [Trait("Deserialize", "Deserializing a new item")]
  public class DeserializeNewItem : DeserializeTestBase
  {
    public DeserializeNewItem()
    {
      this.Db.Add(this.DeserializedItem);
    }

    [Fact(DisplayName = "Creates the new item")]
    public void CreateNewItem()
    {
      this.Db.GetItem(SerializationId.ContentHomeItem).Should().NotBeNull();
    }
  }
}