namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Threading;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  public class AddVersionCommand : Sitecore.Data.Engines.DataCommands.AddVersionCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public AddVersionCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.AddVersionCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.AddVersionCommand, AddVersionCommand>();
    }

    protected override Item DoExecute()
    {
      var dbitem = this.innerCommand.Value.DataStorage.GetFakeItem(this.Item.ID);
      var language = this.Item.Language.Name;
      var currentVersion = Item.Version.Number;
      var nextVersion = new Version(currentVersion + 1);

      foreach (var field in dbitem.Fields)
      {
        var value = field.GetValue(language, currentVersion);
        field.Add(language, value);
      }

      dbitem.VersionsCount[language] = nextVersion.Number;

      return this.innerCommand.Value.DataStorage.GetSitecoreItem(this.Item.ID, this.Item.Language, nextVersion);
    }
  }
}