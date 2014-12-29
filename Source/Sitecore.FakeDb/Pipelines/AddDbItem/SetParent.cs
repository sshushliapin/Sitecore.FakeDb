namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using Sitecore.Data;

  public class SetParent
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;

      if (!ID.IsNullOrEmpty(item.ParentID))
      {
        return;
      }

      if (item.TemplateID == TemplateIDs.Template)
      {
        item.ParentID = ItemIDs.TemplateRoot;
        return;
      }

      if (item.TemplateID == TemplateIDs.BranchTemplate)
      {
        item.ParentID = ItemIDs.BranchesRoot;
        return;
      }

      item.ParentID = item is DbTemplate ? args.DefaultTemplateRoot : args.DefaultItemRoot;
    }
  }
}