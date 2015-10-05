namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Version = Sitecore.Data.Version;

  public class GetItemCommand : Sitecore.Data.Engines.DataCommands.GetItemCommand
  {
    private readonly DataStorage dataStorage;

    public GetItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.GetItemCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override Item DoExecute()
    {
      var version = this.Version;
      if (version == Version.Latest)
      {
        var item = this.DataStorage.GetFakeItem(this.ItemId);
        if (item != null)
        {
          var language = this.Language.Name;
          version = Version.Parse(item.GetVersionCount(language));
        }
      }

      return this.dataStorage.GetSitecoreItem(this.ItemId, this.Language, version);
    }
  }
}