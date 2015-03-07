namespace Sitecore.FakeDb.Serialization.Pipelines
{
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Pipelines;

  using Sitecore.Data;

  public class CopySharedFields
  {
    public void Process(DsItemLoadingArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      args.DsDbItem.SyncItem.CopySharedFieldsTo(args.DsDbItem);

      var template = args.DsDbItem as DbTemplate;
      if (template != null)
      {
        template.BaseIDs = new[] { ID.Parse(template.Fields[FieldIDs.BaseTemplate].Value) };
      }
    }
  }
}