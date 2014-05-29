namespace Sitecore.FakeDb.Links
{
  using System;
  using System.Threading;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Links;

  public class SwitchingLinkDatabase : LinkDatabase, IDisposable
  {
    private readonly ThreadLocal<LinkDatabase> behavior = new ThreadLocal<LinkDatabase>();

    public LinkDatabase Behavior
    {
      get { return this.behavior.Value; }
      set { this.behavior.Value = value; }
    }

    public override void Compact(Database database)
    {
      this.Behavior.Compact(database);
    }

    public override ItemLink[] GetBrokenLinks(Database database)
    {
      return this.Behavior.GetBrokenLinks(database);
    }

    public override int GetReferenceCount(Item item)
    {
      return this.Behavior.GetReferenceCount(item);
    }

    public override ItemLink[] GetReferences(Item item)
    {
      return this.Behavior.GetReferences(item);
    }

    public override ItemLink[] GetItemReferences(Item item, bool includeStandardValuesLinks)
    {
      return this.Behavior.GetItemReferences(item, includeStandardValuesLinks);
    }

    public override int GetReferrerCount(Item item)
    {
      return this.Behavior.GetReferrerCount(item);
    }

    public override ItemLink[] GetReferrers(Item item)
    {
      return this.Behavior.GetReferrers(item);
    }

    public override ItemLink[] GetReferrers(Item item, ID sourceFieldId)
    {
      return this.Behavior.GetReferrers(item);
    }

    public override ItemLink[] GetItemReferrers(Item item, bool includeStandardValuesLinks)
    {
      return this.Behavior.GetItemReferrers(item, includeStandardValuesLinks);
    }

    public override ItemLink[] GetItemVersionReferrers(Item version)
    {
      return this.Behavior.GetItemVersionReferrers(version);
    }

    [Obsolete("Deprecated - Use GetReferrers(item) instead.")]
    public override ItemLink[] GetReferrers(Item item, bool deep)
    {
      return this.Behavior.GetReferrers(item, deep);
    }

    public override bool HasExternalReferrers(Item item, bool deep)
    {
      return this.Behavior.HasExternalReferrers(item, deep);
    }

    public override void Rebuild(Database database)
    {
      this.Behavior.Rebuild(database);
    }

    public override void RemoveReferences(Item item)
    {
      this.Behavior.RemoveReferences(item);
    }

    public override void UpdateItemVersionReferences(Item item)
    {
      this.Behavior.UpdateItemVersionReferences(item);
    }

    public override void UpdateReferences(Item item)
    {
      this.Behavior.UpdateReferences(item);
    }

    protected override void UpdateLinks(Item item, ItemLink[] links)
    {
    }

    public void Dispose()
    {
      this.Behavior = null;
    }
  }
}