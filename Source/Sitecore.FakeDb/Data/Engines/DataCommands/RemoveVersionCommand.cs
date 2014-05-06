namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;

  public class RemoveVersionCommand : Sitecore.Data.Engines.DataCommands.RemoveVersionCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public void Initialize(DataEngineCommand command)
    {
      this.innerCommand = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.RemoveVersionCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.RemoveVersionCommand, RemoveVersionCommand>();
    }

    protected override bool DoExecute()
    {
      var dbitem = this.innerCommand.DataStorage.GetFakeItem(this.Item.ID);
      var language = this.Item.Language.Name;

      var removed = false;

      foreach (var field in dbitem.Fields)
      {
        if (!field.Values.ContainsKey(language))
        {
          continue;
        }

        var langValues = field.Values[language];
        var lastVersion = langValues.Last();

        langValues.Remove(lastVersion);
        removed = true;
      }

      return removed;
    }
  }
}