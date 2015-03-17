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

      this.InitializeCommand(commands.AddFromTemplatePrototype, dataStorage);
      this.InitializeCommand(commands.AddVersionPrototype, dataStorage);
      this.InitializeCommand(commands.BlobStreamExistsPrototype, dataStorage);
      this.InitializeCommand(commands.CopyItemPrototype, dataStorage);
      this.InitializeCommand(commands.CreateItemPrototype, dataStorage);
      this.InitializeCommand(commands.DeletePrototype, dataStorage);
      this.InitializeCommand(commands.GetBlobStreamPrototype, dataStorage);
      this.InitializeCommand(commands.GetChildrenPrototype, dataStorage);
      this.InitializeCommand(commands.GetItemPrototype, dataStorage);
      this.InitializeCommand(commands.GetParentPrototype, dataStorage);
      this.InitializeCommand(commands.GetRootItemPrototype, dataStorage);
      this.InitializeCommand(commands.GetVersionsPrototype, dataStorage);
      this.InitializeCommand(commands.HasChildrenPrototype, dataStorage);
      this.InitializeCommand(commands.MoveItemPrototype, dataStorage);
      this.InitializeCommand(commands.RemoveVersionPrototype, dataStorage);
      this.InitializeCommand(commands.ResolvePathPrototype, dataStorage);
      this.InitializeCommand(commands.SaveItemPrototype, dataStorage);
      this.InitializeCommand(commands.SetBlobStreamPrototype, dataStorage);
    }

    protected virtual void InitializeCommand(object command, DataStorage dataStorage)
    {
      var cmd = command as IDataEngineCommand;
      if (cmd != null)
      {
        cmd.Initialize(dataStorage);
      }
    }
  }
}