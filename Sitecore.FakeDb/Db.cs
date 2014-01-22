namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Templates;
  using Sitecore.SecurityModel;

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
      newItem.RuntimeSettings.Temporary = true;

      using (new EditContext(newItem, SecurityCheck.Disable))
      {
        foreach (var field in item.Fields)
        {
          newItem[field.Key] = field.Value.ToString();
        }
      }
    }

    private void CreateTemplateIfMissing(FItem item)
    {
      var dataStorage = this.database.GetDataStorage();
      if (dataStorage.FakeTemplates.ContainsKey(item.TemplateID))
      {
        return;
      }

      var fields = new Dictionary<string, ID>(item.Fields.Count);
      foreach (var field in item.Fields)
      {
        fields.Add(field.Key, ID.NewID);
      }

      dataStorage.FakeTemplates.Add(item.TemplateID, new FTemplate(item.Name, item.TemplateID) { Fields = fields });
      }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}