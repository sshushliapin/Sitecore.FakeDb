namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
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

    private readonly Database database;

    private readonly DataStorage dataStorage;

    private readonly DbConfiguration configuration;

    private readonly PipelineWatcher pipelineWatcher;

    private bool disposed;

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
      : this()
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
      this.SetParent(item);
      this.EnsureIsChild(item);
      this.SetFullPath(item);
      this.CreateItem(item);
      this.CreateChildren(item);
      this.SetAccess(item);
    }

    public void Add(DbTemplate template)
    {
      Assert.ArgumentNotNull(template, "template");

      if (ID.IsNullOrEmpty(template.ID))
      {
        template.ID = ID.NewID;
      }
      else
      {
        Assert.ArgumentCondition(!this.DataStorage.FakeTemplates.ContainsKey(template.ID), "template", "A tamplete with the same id has already been added.");
      }

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
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (!disposing)
      {
        return;
      }

      CorePipeline.Run("releaseFakeDb", new DbArgs(this));

      this.disposed = true;
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

      if (item.TemplateID == ID.Null)
      {
        item.TemplateID = ID.NewID;
      }

      var fields = new DbFieldCollection();
      foreach (var itemField in item.Fields)
      {
        var templatefield = new DbField(itemField.Name, itemField.ID) { Type = itemField.Type };
        fields.Add(templatefield);
      }

      var template = new DbTemplate(item.Name, item.TemplateID, fields);

      this.Add(template);
    }

    protected virtual bool ResolveTemplate(DbItem item)
    {
      if (this.dataStorage.FakeTemplates.ContainsKey(item.TemplateID))
      {
        var template = this.dataStorage.FakeTemplates[item.TemplateID];
        MapFields(template.Fields, item);

        return true;
      }

      if (!ID.IsNullOrEmpty(item.TemplateID))
      {
        return false;
      }

      var sourceItem = this.DataStorage.FakeItems.Values.LastOrDefault();
      if (sourceItem == null)
      {
        return false;
      }

      if (sourceItem.TemplateID == TemplateIDs.Template)
      {
        return false;
      }


      var lastItemTemplateKeys = string.Concat(sourceItem.Fields.InnerFields.Values.Select(f => f.Name));
      var itemTemplateKeys = string.Concat(item.Fields.InnerFields.Values.Select(f => f.Name));

      if (lastItemTemplateKeys != itemTemplateKeys)
      {
        return false;
      }

      item.TemplateID = sourceItem.TemplateID;

      // TODO:[High] review and redesign.
      MapFields(sourceItem.Fields, item);

      return true;
    }

    private static void MapFields(IEnumerable<DbField> source, DbItem target)
    {
      foreach (var field in source)
      {
        var oldField = target.Fields.InnerFields.Values.SingleOrDefault(v => v.Name == field.Name);
        if (oldField == null)
        {
          continue;
        }

        target.Fields.InnerFields.Remove(oldField.ID);

        var renewedField = oldField;
        renewedField.ID = field.ID;
        target.Fields.InnerFields.Add(field.ID, renewedField);
      }
    }

    protected virtual void SetParent(DbItem item)
    {
      if (ID.IsNullOrEmpty(item.ParentID))
      {
        item.ParentID = DefaultItemRoot;
      }
    }

    protected virtual void EnsureIsChild(DbItem item)
    {
      if (!this.DataStorage.GetFakeItem(item.ParentID).Children.Contains(item))
      {
        this.DataStorage.GetFakeItem(item.ParentID).Children.Add(item);
      }
    }

    protected virtual void SetFullPath(DbItem item)
    {
      if (item.ParentID == DefaultItemRoot)
      {
        item.FullPath = Constants.ContentPath + "/" + item.Name;
        return;
      }

      var parent = this.DataStorage.GetFakeItem(item.ParentID);
      item.FullPath = parent.FullPath + "/" + item.Name;
    }

    protected virtual void CreateItem(DbItem item)
    {
      this.DataStorage.FakeItems.Add(item.ID, item);
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

    protected virtual void SetAccess(DbItem item)
    {
      var fakeItem = this.DataStorage.GetFakeItem(item.ID);

      fakeItem.Access = item.Access;
    }
  }
}