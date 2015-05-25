namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using Sitecore.Exceptions;
  using Sitecore.StringExtensions;

  public class EnsureIsChild
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var dataStorage = args.DataStorage;

      var parentItem = dataStorage.GetFakeItem(item.ParentID);
      if (parentItem == null)
      {
        throw new ItemNotFoundException("The parent item \"{0}\" was not found.".FormatWith(item.ParentID), null);
      }

      if (!parentItem.Children.Contains(item))
      {
        parentItem.Children.Add(item);
      }
    }
  }
}