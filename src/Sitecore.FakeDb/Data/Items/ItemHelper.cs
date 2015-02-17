namespace Sitecore.FakeDb.Data.Items
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;

  public static class ItemHelper
  {
    public static Item CreateInstance()
    {
      return CreateInstance(Database.GetDatabase("master"));
    }

    public static Item CreateInstance(Database database)
    {
      return CreateInstance(database, ID.NewID.ToString());
    }

    public static Item CreateInstance(Database database, ID itemId)
    {
      return CreateInstance(database, ID.NewID.ToString(), itemId);
    }

    public static Item CreateInstance(Database database, string itemName)
    {
      return CreateInstance(database, itemName, ID.NewID);
    }

    public static Item CreateInstance(Database database, string itemName, ID itemId)
    {
      return CreateInstance(database, itemName, itemId, ID.NewID);
    }

    public static Item CreateInstance(Database database, string itemName, ID itemId, ID templateId)
    {
      return CreateInstance(database, itemName, itemId, templateId, ID.Null);
    }

    public static Item CreateInstance(Database database, string itemName, ID itemId, ID templateId, ID branchId)
    {
      return CreateInstance(database, itemName, itemId, templateId, branchId, new FieldList());
    }

    public static Item CreateInstance(Database database, string itemName, ID itemId, ID templateId, ID branchId, FieldList fields)
    {
      return CreateInstance(database, itemName, itemId, templateId, branchId, fields, Language.Current);
    }

    public static Item CreateInstance(Database database, string itemName, ID itemId, ID templateId, ID branchId, FieldList fields, Language language)
    {
      return CreateInstance(database, itemName, itemId, templateId, branchId, fields, language, Version.First);
    }

    public static Item CreateInstance(Database database, string itemName, ID itemId, ID templateId, ID branchId, FieldList fields, Language language, Version version)
    {
      Assert.ArgumentNotNull(database, "database");
      Assert.ArgumentNotNullOrEmpty(itemName, "itemName");
      Assert.ArgumentNotNull(itemId, "itemId");
      Assert.ArgumentNotNull(templateId, "templateId");
      Assert.ArgumentNotNull(fields, "fields");
      Assert.ArgumentNotNull(language, "language");
      Assert.ArgumentNotNull(version, "version");

      var item = new ItemWrapper(itemId, new ItemData(new ItemDefinition(itemId, itemName, templateId, branchId ?? ID.Null), language, version, fields), database);

      EnsureItemFields(item);

      return item;
    }

    internal static void EnsureItemFields(Item item)
    {
      item.Fields.ReadAll();
    }
  }
}