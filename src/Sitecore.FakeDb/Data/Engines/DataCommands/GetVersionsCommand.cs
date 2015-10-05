namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
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
      var item = this.dataStorage.GetFakeItem(this.Item.ID);
      var versions = new VersionCollection();
      if (item == null)
      {
        return versions;
      }

      var versionsCount = item.GetVersionCount(this.Language.Name);
      for (var i = 1; i <= versionsCount; i++)
      {
        versions.Add(new Version(i));
      }

      return versions;
    }
  }
}