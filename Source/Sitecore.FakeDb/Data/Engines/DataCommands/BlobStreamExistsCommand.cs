namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Diagnostics;

  public class BlobStreamExistsCommand : Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    private Guid blobId;

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    // TODO: CMS issue. The base.BlobId property should not be private.
    public override void Initialize(Guid id)
    {
      this.blobId = id;
    }

    protected override Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand, BlobStreamExistsCommand>();
    }

    protected override bool DoExecute()
    {
      // TODO:[Minor] Check what should be returned if there is a blobId exists, but the stream is null.
      return this.innerCommand.DataStorage.Blobs.ContainsKey(this.blobId);
    }
  }
}