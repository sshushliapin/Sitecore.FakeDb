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

    public void Add(DbItem item)
    {
      this.CreateTemplateIfMissing(item);

      var root = this.Database.GetItem(item.ParentID);
      ItemManager.CreateItem(item.Name, root, item.TemplateID, item.ID);

      if (item.Fields.Any())
      {
        var dataStorage = this.Database.GetDataStorage();
        var dbitem = dataStorage.FakeItems[item.ID];
        foreach (var field in item.Fields)
        {
          dbitem.Fields.Add(field);
        }
      }

      foreach (var child in item.Children)
      {
        this.Add(child);
      }
    }

    private void CreateTemplateIfMissing(DbItem item)
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

      dataStorage.FakeTemplates.Add(item.TemplateID, new DbTemplate(item.Name, item.TemplateID) { Fields = fields });
    }

    /// <summary>
    /// Gets the item.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The item.</returns>
    public Item GetItem(string path)
    {
      return this.Database.GetItem(path);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Factory.Reset();
    }
  }
}