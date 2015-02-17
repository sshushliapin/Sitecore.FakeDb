namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Diagnostics;

  public class SetBlobStreamCommand : Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public virtual void Initialize(DataEngineCommand command)
    {
      Assert.ArgumentNotNull(command, "command");

      this.innerCommand = command;
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