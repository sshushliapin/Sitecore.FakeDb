namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class BlobStreamExistsCommand : Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand
  {
    private readonly DataStorage dataStorage;

    private Guid blobId;

    public BlobStreamExistsCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    public override void Initialize(Guid id)
    {
      this.blobId = id;
    }

    protected override Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override bool DoExecute()
    {
      return this.dataStorage.GetBlobStream(this.blobId) != null;
    }
  }
}