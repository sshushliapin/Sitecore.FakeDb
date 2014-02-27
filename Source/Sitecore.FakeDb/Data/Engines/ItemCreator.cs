﻿namespace Sitecore.FakeDb.Data.Engines
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;

  // TODO: To think aboud better name.
  public class ItemCreator
  {
    private readonly DataStorage dataStorage;

    public ItemCreator(DataStorage dataStorage)
    {
      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    public virtual Item Create(string itemName, ID itemId, ID templateId, Database database, Item destination)
    {
      if (this.DataStorage.GetFakeItem(itemId) != null)
      {
        return this.DataStorage.GetSitecoreItem(itemId, destination.Language);
      }

      var fieldList = this.DataStorage.GetFieldList(templateId);
      var item = ItemHelper.CreateInstance(itemName, itemId, templateId, fieldList, database);

      var parentItem = this.DataStorage.GetFakeItem(destination.ID);
      var fullPath = parentItem.FullPath + "/" + itemName;
      var dbitem = new DbItem(itemName, itemId, templateId) { ParentID = destination.ID, FullPath = fullPath };

      this.DataStorage.FakeItems.Add(itemId, dbitem);
      this.DataStorage.GetFakeItem(destination.ID).Children.Add(dbitem);

      return item;
    }
  }
}