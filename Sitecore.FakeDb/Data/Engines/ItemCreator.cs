namespace Sitecore.FakeDb.Data.Engines
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;

  // TODO: To think aboud better name.
  public class ItemCreator
  {
    public virtual Item Create(string itemName, ID itemId, ID templateId, Database database, Item destination)
    {
      var dataStorage = database.GetDataStorage();

      var fieldList = dataStorage.GetFieldList(templateId);
      var item = ItemHelper.CreateInstance(itemName, itemId, templateId, fieldList, database);

      var fullPath = destination.Paths.FullPath + "/" + itemName;
      dataStorage.FakeItems.Add(itemId, new FItem(itemName, itemId, templateId) { ParentID = destination.ID, FullPath = fullPath });

      dataStorage.Items.Add(itemId, item);

      return item;
    }
  }
}