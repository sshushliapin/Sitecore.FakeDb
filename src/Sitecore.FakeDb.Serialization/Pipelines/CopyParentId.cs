namespace Sitecore.FakeDb.Serialization.Pipelines
{
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Pipelines;

  public class CopyParentId
  {
    public void Process(DsItemLoadingArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      var syncItem = args.DsDbItem.SyncItem;
      Assert.ArgumentNotNull(syncItem, "SyncItem");
      Assert.ArgumentCondition(ID.IsID(syncItem.ParentID), "ParentID", "Unable to copy ParentId. Valid identifier expected.");

      var parentId = ID.Parse(syncItem.ParentID);
      if (args.DataStorage.GetFakeItem(parentId) == null)
      {
        return;
      }

      // TODO: Avoid type casting.
      ((DbItem)args.DsDbItem).ParentID = parentId;
    }
  }
}