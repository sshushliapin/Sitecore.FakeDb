namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Pipelines;

  public class AddDbItemArgs : PipelineArgs
  {
    private readonly DbItem item;

    private readonly DataStorage dataStorage;

    public AddDbItemArgs(DbItem item, DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.item = item;
      this.dataStorage = dataStorage;
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
  }
}