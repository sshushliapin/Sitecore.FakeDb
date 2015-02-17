namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  public class SetBlobStreamCommand : Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand, SetBlobStreamCommand>();
    }

    protected override bool DoExecute()
    {
      this.innerCommand.DataStorage.Blobs[this.BlobId] = this.Stream;

      return true;
    }
  }
}