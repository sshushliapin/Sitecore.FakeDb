namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data;
  using Sitecore.Data.Engines.DataCommands;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;

  public class FakeCreateItemCommand : CreateItemCommand
  {
    protected override CreateItemCommand CreateInstance()
    {
      return new FakeCreateItemCommand();
    }

    protected override Item DoExecute()
    {
      var definition = new ItemDefinition(ItemId, ItemName, TemplateId, ID.Null);
      var itemData = new ItemData(definition, Language.Invariant, Version.First, new FieldList());
      var item = new Item(this.ItemId, itemData, this.Database);

      var database = (FakeDatabase)this.Database;
      database.DataStorage.Items.Add(ItemId, item);

      return item;
    }
  }
}