namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;

  public class AddFromTemplateCommand : Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand CreateInstance()
    {
      return new AddFromTemplateCommand();
    }

    protected override Item DoExecute()
    {
      var item = ItemHelper.CreateInstance(ItemName, this.NewId, TemplateId, new FieldList(), Database);

      var dataStorage = CommandHelper.GetDataStorage(this);

      var fullPath = Destination.Paths.FullPath + "/" + ItemName;
      dataStorage.FakeItems.Add(NewId, new FItem(ItemName, NewId, TemplateId) { ParentID = Destination.ID, FullPath = fullPath });

      dataStorage.Items.Add(NewId, item);

      return item;
    }
  }
}