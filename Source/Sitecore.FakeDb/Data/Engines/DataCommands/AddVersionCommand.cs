namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class AddVersionCommand : Sitecore.Data.Engines.DataCommands.AddVersionCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.AddVersionCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.AddVersionCommand, AddVersionCommand>();
    }

    protected override Item DoExecute()
    {
      var dbitem = this.innerCommand.DataStorage.GetFakeItem(this.Item.ID);
      var language = this.Item.Language.Name;
      var currentVersion = Item.Version.Number;
      var nextVersion = new Version(currentVersion + 1);

      foreach (var field in dbitem.Fields)
      {
        var value = field.GetValue(language, currentVersion);
        field.Add(language, value);
      }

      dbitem.VersionsCount[language] = nextVersion.Number;

      return this.innerCommand.DataStorage.GetSitecoreItem(this.Item.ID, this.Item.Language, nextVersion);
    }
  }
}