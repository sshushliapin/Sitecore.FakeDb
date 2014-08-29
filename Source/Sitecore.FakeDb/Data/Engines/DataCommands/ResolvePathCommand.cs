namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using System.Threading;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class ResolvePathCommand : Sitecore.Data.Engines.DataCommands.ResolvePathCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public ResolvePathCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.ResolvePathCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.ResolvePathCommand, ResolvePathCommand>();
    }

    protected override ID DoExecute()
    {
      if (ID.IsID(this.ItemPath))
      {
        return new ID(this.ItemPath);
      }

      Assert.IsNotNull(this.innerCommand.Value.DataStorage.FakeItems, "this.innerCommand.Value.DataStorage.FakeItems");

      var itemPath = StringUtil.RemovePostfix("/", this.ItemPath);
      var kvp = this.innerCommand.Value.DataStorage.FakeItems.SingleOrDefault(fi => string.Compare(fi.Value.FullPath, itemPath, StringComparison.OrdinalIgnoreCase) == 0);

      return kvp.Key;
    }
  }
}