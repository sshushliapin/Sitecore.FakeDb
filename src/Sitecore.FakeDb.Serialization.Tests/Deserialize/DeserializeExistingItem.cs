using FluentAssertions;

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
      this.db = new Db { new DbItem("Home Custom", SerializedItemIds.ContentHome) };
      this.deserializedItem = new DsDbItem(SerializedItemIds.ContentHome);
    }

    [Fact(DisplayName = "Does not throw an exception")]
    public void DoesNotThrow()
    {
      var exception = Record.Exception(() => this.db.Add(this.deserializedItem));
      Assert.Null(exception);
    }

    [Fact(DisplayName = "Overwrites the existing item")]
    public void OverwriteExisting()
    {
      var itemId = this.deserializedItem.ID;

      var before = this.db.GetItem(itemId);
      before.ID.ShouldBeEquivalentTo(SerializedItemIds.ContentHome);
      before.Name.ShouldBeEquivalentTo("Home Custom");

      this.db.Add(this.deserializedItem);

      var after = this.db.GetItem(itemId);
      after.ID.ShouldBeEquivalentTo(SerializedItemIds.ContentHome);
      after.Name.ShouldBeEquivalentTo("Home");      
    }

    public void Dispose()
    {
      this.db.Dispose();
    }
  }
}