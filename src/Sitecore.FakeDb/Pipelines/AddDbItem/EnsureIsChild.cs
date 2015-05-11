namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  public class EnsureIsChild
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var dataStorage = args.DataStorage;

      if (!dataStorage.GetFakeItem(item.ParentID).Children.Contains(item))
      {
        dataStorage.GetFakeItem(item.ParentID).Children.Add(item);
      }
    }
  }
}