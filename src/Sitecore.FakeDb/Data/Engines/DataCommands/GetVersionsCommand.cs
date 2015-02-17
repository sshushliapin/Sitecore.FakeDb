namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using Sitecore.Collections;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class GetVersionsCommand : Sitecore.Data.Engines.DataCommands.GetVersionsCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.GetVersionsCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.GetVersionsCommand, GetVersionsCommand>();
    }

    protected override VersionCollection DoExecute()
    {
      var dbitem = this.innerCommand.DataStorage.GetFakeItem(this.Item.ID);
      var language = this.Language.Name;
      var versionsCount = 0;

      if (dbitem.VersionsCount.ContainsKey(language))
      {
        versionsCount = dbitem.VersionsCount[language];
      }

      // TODO:[Minor] Should be moved to independent 'addDbItem' processor.
      foreach (var field in dbitem.Fields)
      {
        if (!field.Values.ContainsKey(language))
        {
          continue;
        }

        var maxVersion = field.Values[language].Keys.OrderBy(k => k).LastOrDefault();
        if (maxVersion > versionsCount)
        {
          versionsCount = maxVersion;
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