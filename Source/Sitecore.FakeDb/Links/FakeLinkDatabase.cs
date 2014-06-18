namespace Sitecore.FakeDb.Links
{
  using System;
  using System.Threading;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Links;

  public class FakeLinkDatabase : LinkDatabase, IBehavioral<LinkDatabase>
  {
    private readonly ThreadLocal<LinkDatabase> behavior = new ThreadLocal<LinkDatabase>();

    public LinkDatabase Behavior
    {
      get { return this.behavior.Value; }
      set { this.behavior.Value = value; }
    }

    public override void Compact(Database database)
    {
      if (this.Behavior != null)
      {
        this.Behavior.Compact(database);
      }
    }

    public override ItemLink[] GetBrokenLinks(Database database)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetBrokenLinks(database);
      }

      return new ItemLink[] { };
    }

    public override int GetReferenceCount(Item item)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetReferenceCount(item);
      }

      return 0;
    }

    public override ItemLink[] GetReferences(Item item)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetReferences(item);
      }

      return new ItemLink[] { };
    }

    public override ItemLink[] GetItemReferences(Item item, bool includeStandardValuesLinks)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetItemReferences(item, includeStandardValuesLinks);
      }

      return new ItemLink[] { };
    }

    public override int GetReferrerCount(Item item)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetReferrerCount(item);
      }

      return 0;
    }

    public override ItemLink[] GetReferrers(Item item)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetReferrers(item);
      }

      return new ItemLink[] { };
    }

    public override ItemLink[] GetReferrers(Item item, ID sourceFieldId)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetReferrers(item);
      }

      return new ItemLink[] { };
    }

    public override ItemLink[] GetItemReferrers(Item item, bool includeStandardValuesLinks)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetItemReferrers(item, includeStandardValuesLinks);
      }

      return new ItemLink[] { };
    }

    public override ItemLink[] GetItemVersionReferrers(Item version)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetItemVersionReferrers(version);
      }

      return new ItemLink[] { };
    }

    [Obsolete("Deprecated - Use GetReferrers(item) instead.")]
    public override ItemLink[] GetReferrers(Item item, bool deep)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetReferrers(item, deep);
      }

      return new ItemLink[] { };
    }

    public override bool HasExternalReferrers(Item item, bool deep)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.HasExternalReferrers(item, deep);
      }

      return false;
    }

    public override void Rebuild(Database database)
    {
      if (this.Behavior != null)
      {
        this.Behavior.Rebuild(database);
      }
    }

    public override void RemoveReferences(Item item)
    {
      if (this.Behavior != null)
      {
        this.Behavior.RemoveReferences(item);
      }
    }

    public override void UpdateItemVersionReferences(Item item)
    {
      if (this.Behavior != null)
      {
        this.Behavior.UpdateItemVersionReferences(item);
      }
    }

    public override void UpdateReferences(Item item)
    {
      if (this.Behavior != null)
      {
        this.Behavior.UpdateReferences(item);
      }
    }

    protected override void UpdateLinks(Item item, ItemLink[] links)
    {
    }
  }
}