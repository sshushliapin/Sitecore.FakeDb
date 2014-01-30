namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Data;

  public class ResolvePathCommand : Sitecore.Data.Engines.DataCommands.ResolvePathCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.ResolvePathCommand CreateInstance()
    {
      return new ResolvePathCommand();
    }

    protected override ID DoExecute()
    {
      var dataStorage = this.Database.GetDataStorage();
      var kvp = dataStorage.FakeItems.SingleOrDefault(fi => string.Compare(fi.Value.FullPath, ItemPath, StringComparison.OrdinalIgnoreCase) == 0);

      return kvp.Key;
    }
  }
}