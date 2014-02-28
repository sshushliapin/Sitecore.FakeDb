namespace Sitecore.FakeDb.Data.Items
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;

  public static class ItemHelper
  {
    public static Item CreateInstance(Database database)
    {
      return CreateInstance(ID.NewID.ToString(), database);
    }

    public static Item CreateInstance(ID itemId, Database database)
    {
      return CreateInstance(ID.NewID.ToString(), itemId, ID.NewID, new FieldList(), database);
    }

    public static Item CreateInstance(string itemName, Database database)
    {
      return CreateInstance(itemName, ID.NewID, ID.NewID, new FieldList(), database);
    }

    public static Item CreateInstance(string itemName, ID itemId, Database database)
    {
      return CreateInstance(itemName, itemId, ID.NewID, new FieldList(), database);
    }

    public static Item CreateInstance(string itemName, ID itemId, ID templateId, FieldList fields, Database database)
    {
      Assert.ArgumentNotNullOrEmpty(itemName, "itemName");
      Assert.ArgumentNotNull(itemId, "itemId");
      Assert.ArgumentNotNull(templateId, "templateId");
      Assert.ArgumentNotNull(fields, "fields");
      Assert.ArgumentNotNull(database, "database");

      return new Item(itemId, new ItemData(new ItemDefinition(itemId, itemName, templateId, ID.Null), Language.Invariant, Version.First, fields), database);
    }
  }
}