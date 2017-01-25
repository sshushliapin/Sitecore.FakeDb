namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Version = Sitecore.Data.Version;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
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
      var currentVersion = dbitem.GetVersionCount(language) != 0 ? this.Item.Version.Number : 0;
      var nextVersion = new Version(currentVersion + 1);

      dbitem.AddVersion(language, currentVersion);

      return this.dataStorage.GetSitecoreItem(this.Item.ID, this.Item.Language, nextVersion);
    }
  }
}