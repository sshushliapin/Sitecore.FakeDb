namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.IO;
  using Sitecore.Diagnostics;

  public class GetBlobStreamCommand : Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand, GetBlobStreamCommand>();
    }

    protected override Stream DoExecute()
    {
      var blobs = this.innerCommand.DataStorage.Blobs;

      return blobs.ContainsKey(this.BlobId) ? blobs[this.BlobId] : null;
    }
  }
}