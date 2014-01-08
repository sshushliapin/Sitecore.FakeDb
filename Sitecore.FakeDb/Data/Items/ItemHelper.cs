namespace Sitecore.FakeDb.Data.Items
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;

  public static class ItemHelper
  {
    public static Item CreateInstance(string itemName)
    {
      return CreateInstance(itemName, ID.NewID);
    }

    public static Item CreateInstance(string itemName, ID itemId)
    {
      return CreateInstance(itemName, itemId, new FieldList());
    }

    public static Item CreateInstance(string itemName, ID itemId, FieldList fields)
    {
      return CreateInstance(itemName, itemId, ID.NewID, fields, Database.GetDatabase("master"));
    }

    public static Item CreateInstance(string itemName, ID itemId, ID templateId, FieldList fields, Database database)
    {
      return new Item(itemId, new ItemData(new ItemDefinition(itemId, itemName, templateId, ID.Null), Language.Invariant, Version.First, fields), database);
    }
  }
}