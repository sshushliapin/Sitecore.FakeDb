namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using Sitecore.Collections;
  using Sitecore.Configuration;
  using Sitecore.Diagnostics;

  public class GetChildrenCommand : Sitecore.Data.Engines.DataCommands.GetChildrenCommand
  {
    private readonly DataStorage dataStorage;

    public GetChildrenCommand()
      : this((DataStorage)Factory.CreateObject("dataStorage", true))
    {
    }

    public GetChildrenCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.GetChildrenCommand CreateInstance()
    {
      return new GetChildrenCommand(this.DataStorage);
    }

    protected override ItemList DoExecute()
    {
      var item = DataStorage.GetFakeItem(this.Item.ID);
      var itemList = new ItemList();

      itemList.AddRange(item.Children.Select(child => DataStorage.GetSitecoreItem(child.ID, this.Item.Language)));

      return itemList;
    }
  }
}