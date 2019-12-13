namespace Sitecore.FakeDb
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Xml;
    using Sitecore.Common;
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
    using Version = Sitecore.Data.Version;

    /// <summary>
    /// Creates Sitecore items in memory.
    /// </summary>
    public class Db : IDisposable, IEnumerable
    {
        private static readonly object Lock = new object();

        private readonly Database database;

        private readonly DataStorage dataStorage;

        private readonly DataStorageSwitcher dataStorageSwitcher;

        private readonly DatabaseSwitcher databaseSwitcher;

        private readonly Stack<Switcher<DbLanguages>> databaseLanguages;

        private DbConfiguration configuration;

        private PipelineWatcher pipelineWatcher;

        private XmlDocument config;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Db"/> class with the "master" database.
        /// </summary>
        public Db()
            : this("master")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Db"/> class with the specified database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public Db(string databaseName)
        {
            Assert.ArgumentNotNullOrEmpty(databaseName, "databaseName");

            this.database = Database.GetDatabase(databaseName);
            this.dataStorage = new DataStorage(this.database);
            this.dataStorageSwitcher = new DataStorageSwitcher(this.dataStorage);
            this.databaseSwitcher = new DatabaseSwitcher(this.database);
            this.databaseLanguages = new Stack<Switcher<DbLanguages>>();
            this.databaseLanguages.Push(
                new Switcher<DbLanguages>(
                    new DbLanguages(Language.Parse("en"))));

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
            return this.DataStorage.GetFakeItems().GetEnumerator();
        }

        /// <summary>
        /// Adds a <see cref="DbItem" /> to the current database.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(DbItem item)
        {
            Assert.ArgumentNotNull(item, "item");

            this.DataStorage.AddFakeItem(item);
        }

        /// <summary>
        /// Gets an <see cref="Item"/> by id.
        /// </summary>
        /// <param name="id">The item id.</param>
        /// <returns>The item.</returns>
        public Item GetItem(ID id)
        {
            return this.Database.GetItem(id);
        }

        /// <summary>
        /// Gets an <see cref="Item"/> by id and language.
        /// </summary>
        /// <param name="id">The item id.</param>
        /// <param name="language">The item language.</param>
        /// <returns>The item.</returns>
        public Item GetItem(ID id, string language)
        {
            Assert.ArgumentNotNullOrEmpty(language, "language");

            return this.Database.GetItem(id, Language.Parse(language));
        }

        /// <summary>
        /// Gets an <see cref="Item"/> by id, language and version number.
        /// </summary>
        /// <param name="id">The item id.</param>
        /// <param name="language">The item language.</param>
        /// <param name="version">The item version.</param>
        /// <returns>The item.</returns>
        public Item GetItem(ID id, string language, int version)
        {
            Assert.ArgumentNotNullOrEmpty(language, "language");

            return this.Database.GetItem(id, Language.Parse(language), Version.Parse(version));
        }

        /// <summary>
        /// Gets an <see cref="Item" /> by path.
        /// </summary>
        /// <param name="path">The item path.</param>
        /// <returns>The item.</returns>
        public Item GetItem(string path)
        {
            Assert.ArgumentNotNullOrEmpty(path, "path");

            return this.Database.GetItem(path);
        }

        /// <summary>
        /// Gets an <see cref="Item"/> by path and language.
        /// </summary>
        /// <param name="path">The item path.</param>
        /// <param name="language">The item language.</param>
        /// <returns>The item.</returns>
        public Item GetItem(string path, string language)
        {
            Assert.ArgumentNotNullOrEmpty(path, "path");
            Assert.ArgumentNotNull(language, "language");

            return this.Database.GetItem(path, Language.Parse(language));
        }

        /// <summary>
        /// Gets an <see cref="Item"/> by path, language and version number.
        /// </summary>
        /// <param name="path">The item path.</param>
        /// <param name="language">The item language.</param>
        /// <param name="version">The item version.</param>
        /// <returns>The item.</returns>
        public Item GetItem(string path, string language, int version)
        {
            Assert.ArgumentNotNullOrEmpty(path, "path");
            Assert.ArgumentNotNull(language, "language");

            return this.Database.GetItem(path, Language.Parse(language), Version.Parse(version));
        }

        /// <summary>
        /// Specifies a list of available <see cref="Database"/> languages for 
        /// the given <see cref="Db"/> context. If not called, the 'en' 
        /// language is used.
        /// </summary>
        /// <param name="languages">The list of languages.</param>
        /// <returns>The same <see cref="Db"/> instance.</returns>
        public Db WithLanguages(params Language[] languages)
        {
            this.databaseLanguages.Push(
                new Switcher<DbLanguages>(
                    new DbLanguages(languages)));

            return this;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True if disposing, otherwise false.</param>
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

            this.dataStorageSwitcher.Dispose();
            this.databaseSwitcher.Dispose();
            this.configuration?.Dispose();

            foreach (var languageSwitcher in this.databaseLanguages)
            {
                if (Switcher<DbLanguages>.CurrentValue != null)
                {
                    languageSwitcher.Dispose();
                }
            }

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
