namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using System;

  // TODO: Not used. Remove.
  [Obsolete]
  public class CreateItem
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var dataStorage = args.DataStorage;

      dataStorage.FakeItems.Add(item.ID, item);
    }
  }
}