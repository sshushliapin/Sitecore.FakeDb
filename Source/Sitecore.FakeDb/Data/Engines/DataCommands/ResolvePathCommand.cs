namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class ResolvePathCommand : Sitecore.Data.Engines.DataCommands.ResolvePathCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public virtual void Initialize(DataEngineCommand command)
    {
      Assert.ArgumentNotNull(command, "command");

      this.innerCommand = command;
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

      Assert.IsNotNull(this.innerCommand.DataStorage.FakeItems, "this.innerCommand.Value.DataStorage.FakeItems");

      var itemPath = StringUtil.RemovePostfix("/", this.ItemPath);
      var kvp = this.innerCommand.DataStorage.FakeItems.SingleOrDefault(fi => string.Compare(fi.Value.FullPath, itemPath, StringComparison.OrdinalIgnoreCase) == 0);

      return kvp.Key;
    }
  }
}