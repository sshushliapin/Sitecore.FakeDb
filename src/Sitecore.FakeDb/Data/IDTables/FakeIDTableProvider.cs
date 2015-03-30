namespace Sitecore.FakeDb.Data.IDTables
{
  using System;
  using System.Threading;
  using Sitecore.Data;
  using Sitecore.Data.IDTables;

  public class FakeIDTableProvider : IDTableProvider, IThreadLocalProvider<IDTableProvider>
  {
    private readonly ThreadLocal<IDTableProvider> localProvider = new ThreadLocal<IDTableProvider>();

    private bool disposed;

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

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
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