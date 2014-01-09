namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.FakeDb.Data;

  // TODO: Inherit from Database.
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

      this.CreateTemplateIfMissing(item.Name, item.TemplateID);

      var i = ItemManager.CreateItem(item.Name, root, item.TemplateID, item.ID);

      Diagnostics.Assert.IsNotNull(i, "Unable to create an item.");
    }

    private void CreateTemplateIfMissing(string name, ID templateId)
    {
      var templateItem = this.Database.GetItem(templateId);
      if (templateItem != null)
      {
        return;
      }

      var templatesRoot = this.Database.GetItem(ItemIDs.TemplateRoot);
      ItemManager.CreateItem(name, templatesRoot, TemplateIDs.Template, templateId);
      this.Database.Engines.TemplateEngine.Reset();
    }

    public void Dispose()
    {
      ((FakeDatabase)this.database).DataStorage.Reset();
    }
  }
}