namespace Sitecore.FakeDb.Pipelines
{
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Pipelines;

  public class DsItemLoadingArgs : PipelineArgs
  {
    private readonly IDsDbItem dsDbItem;

    private readonly DataStorage dataStorage;

    public DsItemLoadingArgs(IDsDbItem dsDbItem, DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dsDbItem, "dsDbItem");
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dsDbItem = dsDbItem;
      this.dataStorage = dataStorage;
    }

    public IDsDbItem DsDbItem
    {
      get { return this.dsDbItem; }
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }
  }
}