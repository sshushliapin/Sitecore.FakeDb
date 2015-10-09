namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Version = Sitecore.Data.Version;

  public class AddVersionCommand : Sitecore.Data.Engines.DataCommands.AddVersionCommand
  {
    private readonly DataStorage dataStorage;

    public AddVersionCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.AddVersionCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override Item DoExecute()
    {
      var dbitem = this.dataStorage.GetFakeItem(this.Item.ID);
      var language = this.Item.Language.Name;
      var currentVersion = this.Item.Version.Number;
      var nextVersion = new Version(currentVersion + 1);

      dbitem.AddVersion(language, currentVersion);

      return this.dataStorage.GetSitecoreItem(this.Item.ID, this.Item.Language, nextVersion);
    }
  }
}