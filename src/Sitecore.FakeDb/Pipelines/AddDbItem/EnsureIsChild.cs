namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  public class EnsureIsChild
  {
    public virtual void Process(AddDbItemArgs args)
    {
        if (args.DbItem == null || args.DataStorage == null)
        {
            return;
        }

      var item = args.DbItem;
      var dataStorage = args.DataStorage;

        var fakeItem = dataStorage.GetFakeItem(item.ParentID);

        if (fakeItem == null || fakeItem.Children == null)
        {
            if (fakeItem == null)
            {
                System.Diagnostics.Trace.WriteLine("Can't get parent from data storage - " + item.FullPath);
            }

            if (fakeItem.Children == null)
            {
                System.Diagnostics.Trace.WriteLine("Empty children collection for parent - " + item.FullPath);    
            }

            return;
        }

      if (!fakeItem.Children.Contains(item))
      {
        fakeItem.Children.Add(item);
      }
    }
  }
}