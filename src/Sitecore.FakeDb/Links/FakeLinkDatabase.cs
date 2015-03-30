namespace Sitecore.FakeDb.Links
{
  using System;
  using System.Threading;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Links;

  public class FakeLinkDatabase : LinkDatabase, IThreadLocalProvider<LinkDatabase>
  {
    private readonly ThreadLocal<LinkDatabase> localProvider = new ThreadLocal<LinkDatabase>();

    private readonly ItemLink[] emptyLinks = { };

    private bool disposed;

    public virtual ThreadLocal<LinkDatabase> LocalProvider
    {
      get { return this.localProvider; }
    }

    public override void Compact(Database database)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.Compact(database);
      }
    }

    public override ItemLink[] GetBrokenLinks(Database database)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetBrokenLinks(database) : this.emptyLinks;
    }

    public override ItemLink[] GetItemReferences(Item item, bool includeStandardValuesLinks)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetItemReferences(item, includeStandardValuesLinks) : this.emptyLinks;
    }

    public override ItemLink[] GetItemReferrers(Item item, bool includeStandardValuesLinks)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetItemReferrers(item, includeStandardValuesLinks) : this.emptyLinks;
    }

    public override ItemLink[] GetItemVersionReferrers(Item version)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetItemVersionReferrers(version) : this.emptyLinks;
    }

    public override int GetReferenceCount(Item item)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetReferenceCount(item) : 0;
    }

    public override ItemLink[] GetReferences(Item item)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetReferences(item) : this.emptyLinks;
    }

    public override int GetReferrerCount(Item item)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetReferrerCount(item) : 0;
    }

    public override ItemLink[] GetReferrers(Item item)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetReferrers(item) : this.emptyLinks;
    }

    public override ItemLink[] GetReferrers(Item item, ID sourceFieldId)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetReferrers(item) : this.emptyLinks;
    }

    [Obsolete("Deprecated - Use GetReferrers(item) instead.")]
    public override ItemLink[] GetReferrers(Item item, bool deep)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetReferrers(item, deep) : this.emptyLinks;
    }

    public override bool HasExternalReferrers(Item item, bool deep)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.HasExternalReferrers(item, deep);
    }

    public virtual bool IsLocalProviderSet()
    {
      return this.localProvider.Value != null;
    }

    public override void Rebuild(Database database)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.Rebuild(database);
      }
    }

    public override void RemoveReferences(Item item)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.RemoveReferences(item);
      }
    }

    public override void UpdateItemVersionReferences(Item item)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.UpdateItemVersionReferences(item);
      }
    }

    public override void UpdateReferences(Item item)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.UpdateReferences(item);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected override void UpdateLinks(Item item, ItemLink[] links)
    {
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (!disposing)
      {
        return;
      }

      this.localProvider.Dispose();

      this.disposed = true;
    }
  }
}