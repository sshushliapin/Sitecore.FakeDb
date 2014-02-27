namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class ResolvePathCommand : Sitecore.Data.Engines.DataCommands.ResolvePathCommand
  {
    private readonly DataStorage dataStorage;

    public ResolvePathCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.ResolvePathCommand CreateInstance()
    {
      return new ResolvePathCommand(this.DataStorage);
    }

    protected override ID DoExecute()
    {
      var kvp = this.DataStorage.FakeItems.SingleOrDefault(fi => string.Compare(fi.Value.FullPath, ItemPath, StringComparison.OrdinalIgnoreCase) == 0);

      return kvp.Key;
    }
  }
}