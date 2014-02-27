namespace Sitecore.FakeDb.Data
{
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;

  public class FakeDatabase : Database
  {
    public FakeDatabase(string name)
      : base(name)
    {
      //this.Engines.DataEngine.Commands.AddFromTemplatePrototype = new AddFromTemplateCommand();
      this.Engines.DataEngine.Commands.AddVersionPrototype = new AddVersionCommand();
      this.Engines.DataEngine.Commands.BlobStreamExistsPrototype = new BlobStreamExistsCommand();
      this.Engines.DataEngine.Commands.CopyItemPrototype = new CopyItemCommand();
      //this.Engines.DataEngine.Commands.CreateItemPrototype = new CreateItemCommand();
      //this.Engines.DataEngine.Commands.DeletePrototype = new DeleteItemCommand();
      this.Engines.DataEngine.Commands.GetBlobStreamPrototype = new GetBlobStreamCommand();
      //this.Engines.DataEngine.Commands.GetChildrenPrototype = new GetChildrenCommand();
      //this.Engines.DataEngine.Commands.GetItemPrototype = new GetItemCommand();
      //this.Engines.DataEngine.Commands.GetParentPrototype = new GetParentCommand();
      //this.Engines.DataEngine.Commands.GetRootItemPrototype = new GetRootItemCommand();
      this.Engines.DataEngine.Commands.GetVersionsPrototype = new GetVersionsCommand();
      //this.Engines.DataEngine.Commands.HasChildrenPrototype = new HasChildrenCommand();
      this.Engines.DataEngine.Commands.MoveItemPrototype = new MoveItemCommand();
      this.Engines.DataEngine.Commands.RemoveDataPrototype = new RemoveDataCommand();
      this.Engines.DataEngine.Commands.RemoveVersionPrototype = new RemoveVersionCommand();
      //this.Engines.DataEngine.Commands.ResolvePathPrototype = new ResolvePathCommand();
      //this.Engines.DataEngine.Commands.SaveItemPrototype = new SaveItemCommand();
      this.Engines.DataEngine.Commands.SetBlobStreamPrototype = new SetBlobStreamCommand();
    }
  }
}