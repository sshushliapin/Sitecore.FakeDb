using System.Linq;

namespace Sitecore.FakeDb.Data.Engines
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;

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
      var language = Language.Current;

      if (this.DataStorage.GetFakeItem(itemId) == null)
      {
        var parentItem = this.DataStorage.GetFakeItem(destination.ID);
        var fullPath = parentItem.FullPath + "/" + itemName;

        var dbitem = new DbItem(itemName, itemId, templateId) { ParentID = destination.ID, FullPath = fullPath };

        // ToDo [HIGH]: move it out of here and consolidate with the processing that happens in the Db
        SetStatistics(dbitem);

        this.DataStorage.FakeItems.Add(itemId, dbitem);
        this.DataStorage.GetFakeItem(destination.ID).Children.Add(dbitem);
      }

      return this.DataStorage.GetSitecoreItem(itemId, language);
    }

    protected void SetStatistics(DbItem item)
    {
      var date = DateUtil.IsoNow;
      var user = Context.User.Name;

      item.Fields.Add(new DbField("__Created", FieldIDs.Created) { Value = date });
      item.Fields.Add(new DbField("__Created by", FieldIDs.CreatedBy) { Value = user });
      item.Fields.Add(new DbField("__Revision", FieldIDs.Revision) { Value = ID.NewID.ToString() });
      item.Fields.Add(new DbField("__Updated", FieldIDs.Updated) { Value = date });
      item.Fields.Add(new DbField("__Updated by", FieldIDs.UpdatedBy) { Value = user });
    }
  }
}