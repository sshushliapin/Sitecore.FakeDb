namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using Sitecore.Data;
  using Sitecore.Data.Managers;
  using Sitecore.FakeDb.Data;

  public class Db : IDisposable, IEnumerable
  {
    private readonly Database database;

    public Db()
    {
      this.database = Database.GetDatabase("master");
    }

    public Database Database
    {
      get { return this.database; }
    }

    public IEnumerator GetEnumerator()
    {
      throw new NotImplementedException();
    }

    public void Add(FItem item)
    {
      var root = this.Database.GetItem(ItemIDs.ContentRoot);

      var i = ItemManager.CreateItem(item.Name, root, item.TemplateID, item.ID);
      Diagnostics.Assert.IsNotNull(i, "Unable to create an item.");
    }

    public void Dispose()
    {
      ((FakeDatabase)this.database).DataStorage.Reset();
    }
  }
}