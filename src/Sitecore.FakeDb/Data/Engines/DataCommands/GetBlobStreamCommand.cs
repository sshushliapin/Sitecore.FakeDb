namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.IO;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class GetBlobStreamCommand : Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand
  {
    private readonly DataStorage dataStorage;

    public GetBlobStreamCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override Stream DoExecute()
    {
      return this.dataStorage.GetBlobStream(this.BlobId);
    }
  }
}