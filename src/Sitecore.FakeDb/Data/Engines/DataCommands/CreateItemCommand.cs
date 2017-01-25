namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
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
      item.RemoveVersion(Language.Current.Name);

      return this.dataStorage.GetSitecoreItem(this.ItemId);
    }
  }
}