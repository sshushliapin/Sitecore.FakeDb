namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using Sitecore.Collections;
  using Sitecore.Data;
  using System.Threading;

  public class GetVersionsCommand : Sitecore.Data.Engines.DataCommands.GetVersionsCommand, IDataEngineCommand
  {
    private ThreadLocal<DataEngineCommand> innerCommand;

    public GetVersionsCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand>();
      this.innerCommand.Value = DataEngineCommand.NotInitialized;
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.GetVersionsCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.GetVersionsCommand, GetVersionsCommand>();
    }

    protected override VersionCollection DoExecute()
    {
      var dbitem = this.innerCommand.Value.DataStorage.GetFakeItem(this.Item.ID);
      int versionsCount = 1;

      foreach (var field in dbitem.Fields)
      {
        var language = this.Language.Name;
        if (field.Values.ContainsKey(language))
        {
          var maxVersion = field.Values[language].Keys.OrderBy(k => k).LastOrDefault();
          if (maxVersion > versionsCount)
          {
            versionsCount = maxVersion;
          }
        }
      }

      var versions = new VersionCollection();
      for (var i = 1; i <= versionsCount; i++)
      {
        versions.Add(new Version(i));
      }

      return versions;
    }
  }
}