namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  public class SetFullPath
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var dataStorage = args.DataStorage;

      if (item.ParentID == args.DefaultItemRoot)
      {
        item.FullPath = Constants.ContentPath + "/" + item.Name;
        return;
      }

      var parent = dataStorage.GetFakeItem(item.ParentID);
      item.FullPath = parent.FullPath + "/" + item.Name;
    }
  }
}