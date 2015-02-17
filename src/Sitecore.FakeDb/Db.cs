namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using System.Threading;
  using System.Xml;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Configuration;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines;
  using Sitecore.FakeDb.Pipelines.InitFakeDb;
  using Sitecore.FakeDb.Pipelines.ReleaseFakeDb;
  using Sitecore.Globalization;
  using Sitecore.Pipelines;

  public class Db : IDisposable, IEnumerable
  {
    private static readonly object Lock = new object();

    private readonly Database database;

    private readonly DataStorage dataStorage;

    private readonly DatabaseSwitcher databaseSwitcher;

    private DbConfiguration configuration;

    private PipelineWatcher pipelineWatcher;

    private XmlDocument config;

    private bool disposed;

    public Db()
      : this("master")
    {
    }

    public Db(string databaseName)
    {
      Assert.ArgumentNotNullOrEmpty(databaseName, "databaseName");

      this.database = Database.GetDatabase(databaseName);
      this.dataStorage = new DataStorage(this.database);

      this.databaseSwitcher = new DatabaseSwitcher(this.database);

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
      get
      {
        if (this.configuration != null)
        {
          return this.configuration;
        }

        this.config = this.GetConfiguration();
        this.configuration = new DbConfiguration(this.config);

        return this.configuration;
      }
    }

    public PipelineWatcher PipelineWatcher
    {
      get
      {
        if (this.pipelineWatcher != null)
        {
          return this.pipelineWatcher;
        }

        this.config = this.GetConfiguration();
        this.pipelineWatcher = new PipelineWatcher(this.config);

        return this.pipelineWatcher;
      }
    }

    protected internal DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    public IEnumerator GetEnumerator()
    {
      throw new NotImplementedException();
    }

    public void Add(DbItem item)
    {
      Assert.ArgumentNotNull(item, "item");

      this.DataStorage.AddFakeItem(item);
    }

    public void Add(DbTemplate template)
    {
      Assert.ArgumentNotNull(template, "template");
      Assert.ArgumentCondition(!this.DataStorage.FakeTemplates.ContainsKey(template.ID), "template", "A template with the same id has already been added.");

      this.DataStorage.AddFakeTemplate(template);
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

      CorePipeline.Run("releaseFakeDb", new ReleaseDbArgs(this));

      this.databaseSwitcher.Dispose();

      if (Monitor.IsEntered(Lock))
      {
        Monitor.Exit(Lock);
      }

      this.disposed = true;
    }

    private XmlDocument GetConfiguration()
    {
      if (this.config != null)
      {
        return this.config;
      }

      Monitor.Enter(Lock);
      this.config = Factory.GetConfiguration();

      return this.config;
    }
  }
}