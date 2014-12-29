namespace Sitecore.FakeDb.Data.IDTables
{
  using System.Threading;
  using Sitecore.Data;
  using Sitecore.Data.IDTables;

  public class FakeIDTableProvider : IDTableProvider, IThreadLocalProvider<IDTableProvider>
  {
    private readonly ThreadLocal<IDTableProvider> localProvider = new ThreadLocal<IDTableProvider>();

    public virtual ThreadLocal<IDTableProvider> LocalProvider
    {
      get { return this.localProvider; }
    }

    public override void Add(IDTableEntry entry)
    {
      if (this.IsLocalProviderSet())
      {
        this.LocalProvider.Value.Add(entry);
      }
    }

    public override IDTableEntry GetID(string prefix, string key)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetID(prefix, key) : null;
    }

    public override IDTableEntry[] GetKeys(string prefix, ID id)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetKeys(prefix, id) : new IDTableEntry[] { };
    }

    public override void Remove(string prefix, string key)
    {
      if (this.IsLocalProviderSet())
      {
        this.LocalProvider.Value.Remove(prefix, key);
      }
    }

    public bool IsLocalProviderSet()
    {
      return this.localProvider.Value != null;
    }
  }
}