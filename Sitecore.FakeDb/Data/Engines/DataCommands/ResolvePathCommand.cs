namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
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

      var kvp = dataStorage.FakeItems.SingleOrDefault(fi => fi.Value.FullPath == ItemPath);
      
      return kvp.Key;
    }
  }
}