namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using System.Threading;
  using Sitecore.Collections;
  using Sitecore.Data;

  public class GetVersionsCommand : Sitecore.Data.Engines.DataCommands.GetVersionsCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public GetVersionsCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
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
      var language = this.Language.Name;
      var versionsCount = 0;

      if (dbitem.VersionsCount.ContainsKey(language))
      {
        versionsCount = dbitem.VersionsCount[language];
      }

      // TODO:[Minor] Should be moved to independent 'addDbItem' processor.
      foreach (var field in dbitem.Fields)
      {
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