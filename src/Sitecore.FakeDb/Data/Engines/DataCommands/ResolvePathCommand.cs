namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class ResolvePathCommand : Sitecore.Data.Engines.DataCommands.ResolvePathCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.ResolvePathCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.ResolvePathCommand, ResolvePathCommand>();
    }

    protected override ID DoExecute()
    {
      if (ID.IsID(this.ItemPath))
      {
        return new ID(this.ItemPath);
      }

      var itemPath = StringUtil.RemovePostfix("/", this.ItemPath);
      var item = this.innerCommand.DataStorage.GetFakeItems().SingleOrDefault(fi => string.Compare(fi.FullPath, itemPath, StringComparison.OrdinalIgnoreCase) == 0);

      return item != null ? item.ID : null;
    }
  }
}