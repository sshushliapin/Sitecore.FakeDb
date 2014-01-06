namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Engines.DataCommands;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;

  public class FakeCreateItemCommand : CreateItemCommand
  {
    protected override CreateItemCommand CreateInstance()
    {
      return new FakeCreateItemCommand();
    }

    protected override Item DoExecute()
    {
      var item = ItemHelper.CreateInstance(ItemName, ItemId, TemplateId, Database);

      var database = (FakeDatabase)this.Database;
      database.DataStorage.Items.Add(ItemId, item);

      return item;
    }
  }
}