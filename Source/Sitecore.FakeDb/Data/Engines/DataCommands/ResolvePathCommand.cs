namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class ResolvePathCommand : Sitecore.Data.Engines.DataCommands.ResolvePathCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.ResolvePathCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.ResolvePathCommand, ResolvePathCommand>();
    }

    protected override ID DoExecute()
    {
      var kvp = this.innerCommand.DataStorage.FakeItems.SingleOrDefault(fi => string.Compare(fi.Value.FullPath, ItemPath, StringComparison.OrdinalIgnoreCase) == 0);

      return kvp.Key;
    }
  }
}