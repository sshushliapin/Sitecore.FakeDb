namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class CreateItemCommand : Sitecore.Data.Engines.DataCommands.CreateItemCommand
  {
    private readonly DataStorage dataStorage;

    public CreateItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.CreateItemCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override Item DoExecute()
    {
      var item = new DbItem(this.ItemName, this.ItemId, this.TemplateId) { ParentID = this.Destination.ID };
      this.dataStorage.AddFakeItem(item);
      item.VersionsCount.Clear();

      return this.dataStorage.GetSitecoreItem(this.ItemId);
    }
  }
}