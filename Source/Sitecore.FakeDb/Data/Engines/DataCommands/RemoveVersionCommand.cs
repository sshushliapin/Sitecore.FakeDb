namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using System.Threading;

  public class RemoveVersionCommand : Sitecore.Data.Engines.DataCommands.RemoveVersionCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public RemoveVersionCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.RemoveVersionCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.RemoveVersionCommand, RemoveVersionCommand>();
    }

    protected override bool DoExecute()
    {
      var dbitem = this.innerCommand.Value.DataStorage.GetFakeItem(this.Item.ID);
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

        removed = langValues.Remove(lastVersion);
      }

      if (dbitem.VersionsCount.ContainsKey(language))
      {
        dbitem.VersionsCount[language] -= 1;
        removed = true;
      }

      return removed;
    }
  }
}