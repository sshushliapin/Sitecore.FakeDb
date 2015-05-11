namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using System;

  public abstract class DeserializeTestBase : IDisposable
  {
    protected Db Db { get; private set; }

    protected DbItem AdhocItem { get; private set; }

    protected DbItem DeserializedItem { get; private set; }

    protected DeserializeTestBase()
    {
      this.Db = new Db();
      this.AdhocItem = new DbItem("Home", SerializedItemIds.ContentHome) { { "Title", "Home" } };
      this.DeserializedItem = new DsDbItem(SerializedItemIds.ContentHome);
    }

    public void Dispose()
    {
      this.Db.Dispose();
    }
  }
}