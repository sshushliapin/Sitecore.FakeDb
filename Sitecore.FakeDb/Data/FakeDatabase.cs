namespace Sitecore.FakeDb.Data
{
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Engines;

  public class FakeDatabase : Database
  {
    public FakeDatabase(string name)
      : base(name)
    {
      this.Engines.DataEngine.Commands.AddFromTemplatePrototype = new AddFromTemplateCommand();
      this.Engines.DataEngine.Commands.AddVersionPrototype = new AddVersionCommand();
      this.Engines.DataEngine.Commands.CreateItemPrototype = new CreateItemCommand();
      this.Engines.DataEngine.Commands.GetItemPrototype = new GetItemCommand();
      this.Engines.DataEngine.Commands.GetParentPrototype = new GetParentCommand();
      this.Engines.DataEngine.Commands.GetRootItemPrototype = new GetRootItemCommand();
      this.Engines.DataEngine.Commands.HasChildrenPrototype = new HasChildrenCommand();
      this.Engines.DataEngine.Commands.ResolvePathPrototype = new ResolvePathCommand();
      this.Engines.DataEngine.Commands.SaveItemPrototype = new SaveItemCommand();

      DataStorage = new DataStorage();
      DataStorage.SetDatabase(this);
    }

    public DataStorage DataStorage { get; set; }
  }
}