namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Collections;
  using Sitecore.Diagnostics;
  using Version = Sitecore.Data.Version;

  public class GetVersionsCommand : Sitecore.Data.Engines.DataCommands.GetVersionsCommand
  {
    private readonly DataStorage dataStorage;

    public GetVersionsCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.GetVersionsCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override VersionCollection DoExecute()
    {
      var dbitem = this.dataStorage.GetFakeItem(this.Item.ID);
      var language = this.Language.Name;
      var versionsCount = 0;

      if (dbitem.VersionsCount.ContainsKey(language))
      {
        versionsCount = dbitem.VersionsCount[language];
      }

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