namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Threading;

  public class BlobStreamExistsCommand : Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    private Guid blobId;

    public BlobStreamExistsCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    // TODO: CMS issue. The base.BlobId property should not be private.
    public override void Initialize(Guid id)
    {
      this.blobId = id;
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand, BlobStreamExistsCommand>();
    }

    protected override bool DoExecute()
    {
      // TODO:[Minor] Check what should be returned if there is a blobId exists, but the stream is null.
      return this.innerCommand.Value.DataStorage.Blobs.ContainsKey(this.blobId);
    }
  }
}