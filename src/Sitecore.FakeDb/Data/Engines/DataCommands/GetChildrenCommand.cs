namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Collections;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class GetChildrenCommand : Sitecore.Data.Engines.DataCommands.GetChildrenCommand
  {
    private readonly DataStorage dataStorage;

    public GetChildrenCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.GetChildrenCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override ItemList DoExecute()
    {
      var item = this.dataStorage.GetFakeItem(this.Item.ID);
      var itemList = new ItemList();

      if (item == null)
      {
        return itemList;
      }

      var children = item.Children.Select(child => this.dataStorage.GetSitecoreItem(child.ID, this.Item.Language));
      itemList.AddRange(children);

      return itemList;
    }
  }
}