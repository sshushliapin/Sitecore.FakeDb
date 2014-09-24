namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  using Sitecore.FakeDb.Data.Engines.DataCommands;

  public class InitDataEngineCommands : InitDbProcessor
  {
    public override void Process(InitDbArgs args)
    {
      var commands = args.Database.Engines.DataEngine.Commands;
      var innerCommand = new DataEngineCommand(args.DataStorage);

      this.InitializeCommand(commands.AddFromTemplatePrototype, innerCommand);
      this.InitializeCommand(commands.AddVersionPrototype, innerCommand);
      this.InitializeCommand(commands.CopyItemPrototype, innerCommand);
      this.InitializeCommand(commands.CreateItemPrototype, innerCommand);
      this.InitializeCommand(commands.DeletePrototype, innerCommand);
      this.InitializeCommand(commands.GetChildrenPrototype, innerCommand);
      this.InitializeCommand(commands.GetItemPrototype, innerCommand);
      this.InitializeCommand(commands.GetParentPrototype, innerCommand);
      this.InitializeCommand(commands.GetRootItemPrototype, innerCommand);
      this.InitializeCommand(commands.GetVersionsPrototype, innerCommand);
      this.InitializeCommand(commands.HasChildrenPrototype, innerCommand);
      this.InitializeCommand(commands.MoveItemPrototype, innerCommand);
      this.InitializeCommand(commands.RemoveVersionPrototype, innerCommand);
      this.InitializeCommand(commands.ResolvePathPrototype, innerCommand);
      this.InitializeCommand(commands.SaveItemPrototype, innerCommand);
    }

    protected virtual void InitializeCommand(object command, DataEngineCommand innerCommand)
    {
      var cmd = command as IDataEngineCommand;
      if (cmd != null)
      {
        cmd.Initialize(innerCommand);
      }
    }
  }
}