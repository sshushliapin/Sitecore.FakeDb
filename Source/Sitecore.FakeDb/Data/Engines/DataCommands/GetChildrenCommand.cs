namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using Sitecore.Collections;

  public class GetChildrenCommand : Sitecore.Data.Engines.DataCommands.GetChildrenCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.GetChildrenCommand CreateInstance()
    {
      return new GetChildrenCommand();
    }

    protected override ItemList DoExecute()
    {
      var dataStorage = this.Database.GetDataStorage();

      var item = dataStorage.GetFakeItem(Item.ID);
      var itemList = new ItemList();
      
      itemList.AddRange(item.Children.Select(child => dataStorage.GetSitecoreItem(child.ID)));

      return itemList;
    }
  }
}