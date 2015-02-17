namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;

  public class InitDataEngineCommands : InitDbProcessor
  {
    public override void Process(InitDbArgs args)
    {
      var commands = args.Database.Engines.DataEngine.Commands;
      var dataStorage = args.DataStorage;

      var innerCommand = new DataEngineCommand();

      this.InitializeCommand(commands.AddFromTemplatePrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.AddVersionPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.BlobStreamExistsPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.CopyItemPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.CreateItemPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.DeletePrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.GetBlobStreamPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.GetChildrenPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.GetItemPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.GetParentPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.GetRootItemPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.GetVersionsPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.HasChildrenPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.MoveItemPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.RemoveVersionPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.ResolvePathPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.SaveItemPrototype, innerCommand, dataStorage);
      this.InitializeCommand(commands.SetBlobStreamPrototype, innerCommand, dataStorage);
    }

    protected virtual void InitializeCommand(object command, DataEngineCommand innerCommand, DataStorage dataStorage)
    {
      var cmd = command as IDataEngineCommand;
      if (cmd != null)
      {
        cmd.Initialize(dataStorage);
      }
    }
  }
}