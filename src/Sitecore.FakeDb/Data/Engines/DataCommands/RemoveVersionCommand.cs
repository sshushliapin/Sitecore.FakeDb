namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class RemoveVersionCommand : Sitecore.Data.Engines.DataCommands.RemoveVersionCommand
  {
    private readonly DataStorage dataStorage;

    public RemoveVersionCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.RemoveVersionCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override bool DoExecute()
    {
      var dbitem = this.dataStorage.GetFakeItem(this.Item.ID);
      var language = this.Item.Language.Name;
      var version = this.Item.Version.Number;

      return dbitem.RemoveVersion(language, version);
    }
  }
}