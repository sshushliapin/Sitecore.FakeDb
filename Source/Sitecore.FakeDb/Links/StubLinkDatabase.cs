namespace Sitecore.FakeDb.Links
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Links;

  public class StubLinkDatabase : LinkDatabase
  {
    private readonly ItemLink[] emptyLinks = { };

    public override void Compact(Database database)
    {
    }

    public override ItemLink[] GetBrokenLinks(Database database)
    {
      return this.emptyLinks;
    }

    public override int GetReferenceCount(Item item)
    {
      return 0;
    }

    public override ItemLink[] GetReferences(Item item)
    {
      return this.emptyLinks;
    }

    public override ItemLink[] GetItemReferences(Item item, bool includeStandardValuesLinks)
    {
      return this.emptyLinks;
    }

    public override int GetReferrerCount(Item item)
    {
      return 0;
    }

    public override ItemLink[] GetReferrers(Item item)
    {
      return this.emptyLinks;
    }

    public override ItemLink[] GetReferrers(Item item, ID sourceFieldId)
    {
      return this.emptyLinks;
    }

    public override ItemLink[] GetItemReferrers(Item item, bool includeStandardValuesLinks)
    {
      return this.emptyLinks;
    }

    public override ItemLink[] GetItemVersionReferrers(Item version)
    {
      return this.emptyLinks;
    }

    public override ItemLink[] GetReferrers(Item item, bool deep)
    {
      return this.emptyLinks;
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

    protected override void UpdateLinks(Item item, ItemLink[] links)
    {
    }
  }
}