namespace Sitecore.FakeDb.Serialization.Pipelines
{
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Pipelines;

  public class CopySharedFields
  {
    public void Process(DsItemLoadingArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      args.DsDbItem.SyncItem.CopySharedFieldsTo(args.DsDbItem);
    }
  }
}