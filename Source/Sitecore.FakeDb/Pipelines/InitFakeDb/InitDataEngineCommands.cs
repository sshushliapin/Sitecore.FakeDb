namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  public class InitDataEngineCommands : InitDbProcessor
  {
    public override void Process(InitDbArgs args)
    {
      var commands = args.Database.Engines.DataEngine.Commands;
      var dataStorage = args.DataStorage;

      this.SetDataStorage(commands.AddFromTemplatePrototype, dataStorage);
      this.SetDataStorage(commands.CreateItemPrototype, dataStorage);
      this.SetDataStorage(commands.DeletePrototype, dataStorage);
      this.SetDataStorage(commands.GetChildrenPrototype, dataStorage);
      this.SetDataStorage(commands.GetItemPrototype, dataStorage);
      this.SetDataStorage(commands.GetParentPrototype, dataStorage);
      this.SetDataStorage(commands.GetRootItemPrototype, dataStorage);
      this.SetDataStorage(commands.HasChildrenPrototype, dataStorage);
      this.SetDataStorage(commands.ResolvePathPrototype, dataStorage);
      this.SetDataStorage(commands.SaveItemPrototype, dataStorage);
    }
  }
}