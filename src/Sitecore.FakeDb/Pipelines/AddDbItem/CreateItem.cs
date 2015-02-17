namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
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