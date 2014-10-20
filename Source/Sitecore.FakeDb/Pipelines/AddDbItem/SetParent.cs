namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using Sitecore.Data;

  public class SetParent
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;

      if (ID.IsNullOrEmpty(item.ParentID))
      {
        item.ParentID = item is DbTemplate ? args.DefaultTemplateRoot : args.DefaultItemRoot;
      }
    }
  }
}