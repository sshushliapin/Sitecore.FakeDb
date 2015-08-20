namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Diagnostics;

  public class RemoveVersionCommand : Sitecore.Data.Engines.DataCommands.RemoveVersionCommand
  {
    private readonly DataStorage dataStorage;

    public RemoveVersionCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.RemoveVersionCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override bool DoExecute()
    {
      var dbitem = this.dataStorage.GetFakeItem(this.Item.ID);
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

      if (!dbitem.VersionsCount.ContainsKey(language))
      {
        return removed;
      }

      dbitem.VersionsCount[language] -= 1;
      return true;
    }
  }
}