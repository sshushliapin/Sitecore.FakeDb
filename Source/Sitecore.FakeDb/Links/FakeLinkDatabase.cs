namespace Sitecore.FakeDb.Links
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;
  using Sitecore.Links;

  public class FakeLinkDatabase : LinkDatabase
  {
    public override void Compact(Database database)
    {
    }

    public override ItemLink[] GetBrokenLinks(Database database)
    {
      return new ItemLink[0];
    }

    public override int GetReferenceCount(Item item)
    {
      return 0;
    }

    public override ItemLink[] GetReferences(Item item)
    {
      return new ItemLink[0];
    }

    public override ItemLink[] GetItemReferences(Item item, bool includeStandardValuesLinks)
    {
      return new ItemLink[0];
    }

    public override int GetReferrerCount(Item item)
    {
      return 0;
    }

    public override ItemLink[] GetReferrers(Item item)
    {
      return new ItemLink[0];
    }

    public override ItemLink[] GetReferrers(Item item, ID sourceFieldId)
    {
      return new ItemLink[0];
    }

    public override ItemLink[] GetItemReferrers(Item item, bool includeStandardValuesLinks)
    {
      return new ItemLink[0];
    }

    public override ItemLink[] GetItemVersionReferrers(Item version)
    {
      return new ItemLink[0];
    }

    public override ItemLink[] GetReferrers(Item item, bool deep)
    {
      return new ItemLink[0];
    }

    public override bool HasExternalReferrers(Item item, bool deep)
    {
      return false;
    }

    public override void Rebuild(Database database)
    {
    }

    public override void RemoveReferences(Item item)
    {
    }

    public override void UpdateItemVersionReferences(Item item)
    {
    }

    public override void UpdateReferences(Item item)
    {
    }

    protected override bool ItemExists(ID itemId, string itemPath, Language itemLanguage, Version itemVersion, Database database)
    {
      return false;
    }

    protected override bool TargetExists(ID targetID, string targetPath, Database database)
    {
      return false;
    }

    protected override void UpdateItemVersionLinks(Item item, ItemLink[] links)
    {
    }

    protected override void UpdateLinks(Item item, ItemLink[] links)
    {
    }
  }
}