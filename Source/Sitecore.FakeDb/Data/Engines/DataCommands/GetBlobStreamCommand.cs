namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.IO;
  using System.Threading;

  public class GetBlobStreamCommand : Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public GetBlobStreamCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand, GetBlobStreamCommand>();
    }

    protected override Stream DoExecute()
    {
      var blobs = this.innerCommand.Value.DataStorage.Blobs;

      return blobs.ContainsKey(this.BlobId) ? blobs[this.BlobId] : null;
    }
  }
}