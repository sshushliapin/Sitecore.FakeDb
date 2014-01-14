namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;

  public class CreateItemCommand : Sitecore.Data.Engines.DataCommands.CreateItemCommand
  {
    private ItemCreator itemCreator;

    public ItemCreator ItemCreator
    {
      get
      {
        return this.itemCreator ?? (itemCreator = new ItemCreator());
      }

      set
      {
        this.itemCreator = value;
      }
    }

    protected override Sitecore.Data.Engines.DataCommands.CreateItemCommand CreateInstance()
    {
      return new CreateItemCommand();
    }

    protected override Item DoExecute()
    {
      return this.ItemCreator.Create(ItemName, ItemId, TemplateId, Database, Destination);
    }
  }
}