namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class ResolvePathCommand : Sitecore.Data.Engines.DataCommands.ResolvePathCommand
  {
    private readonly DataStorage dataStorage;

    public ResolvePathCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.ResolvePathCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override ID DoExecute()
    {
      if (ID.IsID(this.ItemPath))
      {
        return new ID(this.ItemPath);
      }

      var itemPath = StringUtil.RemovePostfix("/", this.ItemPath);
      var item = this.dataStorage.GetFakeItems().FirstOrDefault(fi => string.Compare(fi.FullPath, itemPath, StringComparison.OrdinalIgnoreCase) == 0);

      return item != null ? item.ID : null;
    }
  }
}