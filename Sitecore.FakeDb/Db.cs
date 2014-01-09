namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using System.Linq;
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
      this.CreateTemplateIfMissing(item);

      var root = this.Database.GetItem(ItemIDs.ContentRoot);
      var newItem = ItemManager.CreateItem(item.Name, root, item.TemplateID, item.ID);

      if (!item.Fields.Any())
      {
        return;
      }

      newItem.RuntimeSettings.ForceModified = true;
      newItem.RuntimeSettings.ReadOnlyStatistics = true;

      using (new EditContext(newItem))
      {
        foreach (var field in item.Fields)
        {
          // TODO: Consider using Fields collection here.
          newItem[field.Key] = field.Value.ToString();
        }
      }
    }

    private void CreateTemplateIfMissing(FItem item)
    {
      var templateItem = this.Database.GetItem(item.TemplateID);
      if (templateItem != null)
      {
        return;
      }

      var templatesRoot = this.Database.GetItem(ItemIDs.TemplateRoot);
      templateItem = ItemManager.CreateItem(item.Name, templatesRoot, TemplateIDs.Template, item.TemplateID);
      var sectionItem = templateItem.Add("Data", new TemplateID(TemplateIDs.TemplateSection));

      foreach (var field in item.Fields)
      {
        sectionItem.Add(field.Key, new TemplateID(TemplateIDs.TemplateField));
      }
    }

    public void Dispose()
    {
      ((FakeDatabase)this.database).DataStorage.Reset();
    }
  }
}