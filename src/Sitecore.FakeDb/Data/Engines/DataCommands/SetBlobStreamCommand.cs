namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class SetBlobStreamCommand : Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand
  {
    private readonly DataStorage dataStorage;

    public SetBlobStreamCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override bool DoExecute()
    {
      this.dataStorage.SetBlobStream(this.BlobId, this.Stream);

      return true;
    }
  }
}