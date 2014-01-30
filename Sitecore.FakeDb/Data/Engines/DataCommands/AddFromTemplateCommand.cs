namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;

  public class AddFromTemplateCommand : Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand
  {
    private ItemCreator itemCreator;

    public ItemCreator ItemCreator
    {
      get { return this.itemCreator ?? (this.itemCreator = new ItemCreator()); }
      set { this.itemCreator = value; }
    }

    protected override Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand CreateInstance()
    {
      return new AddFromTemplateCommand();
    }

    protected override Item DoExecute()
    {
      return this.ItemCreator.Create(this.ItemName, this.NewId, this.TemplateId, this.Database, this.Destination);
    }
  }
}