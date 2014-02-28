namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;

  // TODO:[Med] Implement versioning.
  public class AddVersionCommand : Sitecore.Data.Engines.DataCommands.AddVersionCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.AddVersionCommand CreateInstance()
    {
      return new AddVersionCommand();
    }

    protected override Item DoExecute()
    {
      return this.Item;
    }
  }
}