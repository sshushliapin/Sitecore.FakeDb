namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;

  public class CopyItemCommand : Sitecore.Data.Engines.DataCommands.CopyItemCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    private ItemCreator itemCreator;

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand = command;
    }

    public ItemCreator ItemCreator
    {
      get { return this.itemCreator ?? (this.itemCreator = new ItemCreator(this.innerCommand.DataStorage)); }
      set { this.itemCreator = value; }
    }

    protected override Sitecore.Data.Engines.DataCommands.CopyItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.CopyItemCommand, CopyItemCommand>();
    }

    protected override Item DoExecute()
    {
      return ItemCreator.Create(this.CopyName, this.CopyId, this.Source.TemplateID, this.Database, this.Destination);
    }
  }
}