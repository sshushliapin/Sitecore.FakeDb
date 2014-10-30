namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Threading;

  public class SetBlobStreamCommand : Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public SetBlobStreamCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand, SetBlobStreamCommand>();
    }

    protected override bool DoExecute()
    {
      this.innerCommand.Value.DataStorage.Blobs[this.BlobId] = this.Stream;

      return true;
    }
  }
}