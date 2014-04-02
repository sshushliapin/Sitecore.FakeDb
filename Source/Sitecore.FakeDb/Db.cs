namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using System.Linq;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Configuration;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines;
  using Sitecore.FakeDb.Pipelines.InitFakeDb;
  using Sitecore.Globalization;
  using Sitecore.Pipelines;

  public class Db : IDisposable, IEnumerable
  {
    private static readonly ID DefaultItemRoot = ItemIDs.ContentRoot;

    private static readonly ID DefaultTemplateRoot = ItemIDs.TemplateRoot;

    private readonly Database database;

    private readonly DataStorage dataStorage;

    private readonly DbConfiguration configuration;

    private readonly PipelineWatcher pipelineWatcher;

    public Db()
      : this("master")
    {
    }

    public Db(string databaseName)
    {
      this.database = Database.GetDatabase(databaseName);
      this.dataStorage = new DataStorage(this.database);

      var config = Factory.GetConfiguration();
      this.configuration = new DbConfiguration(config);
      this.pipelineWatcher = new PipelineWatcher(config);

      var args = new InitDbArgs(this.database, this.dataStorage);
      CorePipeline.Run("initFakeDb", args);
    }

    internal Db(PipelineWatcher pipelineWatcher)
    {
      this.pipelineWatcher = pipelineWatcher;
    }

    public Database Database
    {
      get { return this.database; }
    }

    public DbConfiguration Configuration
    {
      get { return this.configuration; }
    }

    protected internal DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    public PipelineWatcher PipelineWatcher
    {
      get { return this.pipelineWatcher; }
    }

    public IEnumerator GetEnumerator()
    {
      throw new NotImplementedException();
    }

    public void Add(DbItem item)
    {
      Assert.ArgumentNotNull(item, "item");

      this.CreateTemplate(item);
      this.CreateItem(item);
      this.CreateItemFields(item);
      this.CreateChildren(item);
      this.SetFullPath(item);
      this.SetAccess(item);
    }

    public void Add(DbTemplate template)
    {
      Assert.ArgumentNotNull(template, "template");
      Assert.IsNotNull(template.ID, "template ID");
      Assert.ArgumentCondition(!this.DataStorage.FakeTemplates.ContainsKey(template.ID), "template", "A template with the same id has already been added.");

      this.Add((DbItem)template);
      this.DataStorage.FakeTemplates.Add(template.ID, template);
    }

    public Item GetItem(ID id)
    {
      return this.Database.GetItem(id);
    }

    public Item GetItem(ID id, string language)
    {
      Assert.ArgumentNotNullOrEmpty(language, "language");

      return this.Database.GetItem(id, Language.Parse(language));
    }

    public Item GetItem(ID id, string language, int version)
    {
      Assert.ArgumentNotNullOrEmpty(language, "language");

      return this.Database.GetItem(id, Language.Parse(language), Sitecore.Data.Version.Parse(version));
    }

    public Item GetItem(string path)
    {
      Assert.ArgumentNotNullOrEmpty(path, "path");

      return this.Database.GetItem(path);
    }

    public Item GetItem(string path, string language)
    {
      Assert.ArgumentNotNullOrEmpty(path, "path");
      Assert.ArgumentNotNullOrEmpty(language, "language");

      return this.Database.GetItem(path, Language.Parse(language));
    }

    public Item GetItem(string path, string language, int version)
    {
      Assert.ArgumentNotNullOrEmpty(path, "path");
      Assert.ArgumentNotNullOrEmpty(language, "language");

      return this.Database.GetItem(path, Language.Parse(language), Sitecore.Data.Version.Parse(version));
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.pipelineWatcher.Dispose();
      Factory.Reset();
    }

    protected virtual void CreateTemplate(DbItem item)
    {
      var isResolved = this.ResolveTemplate(item);
      if (isResolved)
      {
        return;
      }

      if (!ID.IsNullOrEmpty(item.TemplateID) && this.DataStorage.GetFakeTemplate(item.TemplateID) != null)
      {
        return;
      }

      item.TemplateID = ID.NewID;

      var fields = new DbFieldCollection();
      foreach (var itemField in item.Fields)
      {
        var templatefield = new DbField(itemField.Name) { ID = itemField.ID };
        fields.Add(templatefield);
      }

      var template = new DbTemplate(item.Name, item.TemplateID) { Fields = fields };

      this.Add(template);
    }

    protected virtual bool ResolveTemplate(DbItem item)
    {
      if (!ID.IsNullOrEmpty(item.TemplateID))
      {
        return false;
      }

      var lastItem = this.DataStorage.FakeItems.Values.LastOrDefault();
      if (lastItem == null || this.IsTemplate(lastItem))
      {
        return false;
      }

      //ToDo: I believe the order of the Values in a normal (not sorted) Dictionary is implementation dependent (~ random)
      var lastItemTemplateKeys = string.Concat(lastItem.Fields.InnerFields.Values.Select(f => f.Name));
      var itemTemplateKeys = string.Concat(item.Fields.InnerFields.Values.Select(f => f.Name));

      if (lastItemTemplateKeys != itemTemplateKeys)
      {
        return false;
      }

      item.TemplateID = lastItem.TemplateID;
      for (var i = 0; i < item.Fields.Count(); i++)
      {
        item.Fields.ElementAt(i).ID = lastItem.Fields.ElementAt(i).ID;
      }

      return true;
    }

    protected virtual void CreateItem(DbItem item)
    {
      if (ID.IsNullOrEmpty(item.ParentID))
      {
        item.ParentID = this.IsTemplate(item) ? DefaultTemplateRoot : DefaultItemRoot;
      }

      var root = this.Database.GetItem(item.ParentID);
      ItemManager.CreateItem(item.Name, root, item.TemplateID, item.ID);
    }

    // Sitecore.Data.Engines.TemplateEngine.IsTemplate
    protected virtual bool IsTemplate(DbItem item)
    {
      Assert.ArgumentNotNull(item, "item");

      return item.TemplateID == TemplateIDs.Template;
    }

    protected virtual void CreateItemFields(DbItem item)
    {
      if (!item.Fields.Any())
      {
        return;
      }

      var dbitem = this.DataStorage.FakeItems[item.ID];
      foreach (var field in item.Fields)
      {
        dbitem.Fields.Add(field);
      }
    }

    protected virtual void CreateChildren(DbItem item)
    {
      foreach (var child in item.Children)
      {
        child.ParentID = item.ID;
        child.FullPath = item.FullPath + "/" + child.Name;
        this.Add(child);
      }
    }

    protected virtual void SetFullPath(DbItem item)
    {
      if (item.ParentID == DefaultItemRoot)
      {
        item.FullPath = Constants.ContentPath + "/" + item.Name;
        return;
      }

      if (item.ParentID == DefaultTemplateRoot)
      {
        // ToDo: move the templates path into constants
        item.FullPath = "/sitecore/templates/" + item.Name;
        return;
      }

      var parent = this.DataStorage.GetFakeItem(item.ParentID);
      item.FullPath = parent.FullPath + "/" + item.Name;
    }

    protected virtual void SetAccess(DbItem item)
    {
      var fakeItem = this.DataStorage.GetFakeItem(item.ID);

      fakeItem.Access = item.Access;
    }
  }
}