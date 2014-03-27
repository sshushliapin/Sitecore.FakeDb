namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  public class AddVersionCommand : Sitecore.Data.Engines.DataCommands.AddVersionCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public void Initialize(DataEngineCommand command)
    {
      this.innerCommand = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.AddVersionCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.AddVersionCommand, AddVersionCommand>();
    }

    protected override Item DoExecute()
    {
      var dbitem = this.innerCommand.DataStorage.GetFakeItem(this.Item.ID);
      var language = this.Item.Language.Name;
      var version = new Version(Item.Version.Number + 1);

      foreach (var field in dbitem.Fields)
      {
        var langValues = field.Values[language];
        var value = langValues.Last().Value;

        langValues.Add(version.Number, value);
      }

      return this.innerCommand.DataStorage.GetSitecoreItem(this.Item.ID, this.Item.Language, version);
    }
  }
}