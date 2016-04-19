namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class HasChildrenCommand : Sitecore.Data.Engines.DataCommands.HasChildrenCommand
  {
    private readonly DataStorage dataStorage;

    public HasChildrenCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.HasChildrenCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override bool DoExecute()
    {
      var fakeItem = this.dataStorage.GetFakeItem(this.Item.ID);

      return fakeItem.Children.Count > 0;
    }
  }
}