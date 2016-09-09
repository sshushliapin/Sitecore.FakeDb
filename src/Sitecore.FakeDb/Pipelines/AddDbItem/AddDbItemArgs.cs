namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Pipelines;
  using Sitecore.Globalization;

  public class AddDbItemArgs : PipelineArgs
  {
    private readonly DbItem item;

    private readonly DataStorage dataStorage;

    private readonly Language language;

    public AddDbItemArgs(DbItem item, DataStorage dataStorage) : this(item, dataStorage, Language.Current)
    {
    }

    public AddDbItemArgs(DbItem item, DataStorage dataStorage, Language language)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(dataStorage, "dataStorage");
      Assert.ArgumentNotNull(language, "language");

      this.item = item;
      this.dataStorage = dataStorage;
      this.language = language;
    }

    public ID DefaultItemRoot
    {
      get { return ItemIDs.ContentRoot; }
    }

    public ID DefaultTemplateRoot
    {
      get { return ItemIDs.TemplateRoot; }
    }

    public DbItem DbItem
    {
      get { return this.item; }
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    public Language Language
    {
      get { return this.language; }
    }
  }
}