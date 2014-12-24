namespace Sitecore.FakeDb.Data
{
  using System.Threading;
  using Sitecore.Data;
  using Sitecore.Data.IDTables;

  public class FakeIDTableProvider : IDTableProvider, IThreadLocalProvider<IDTableProvider>
  {
    public override void Add(IDTableEntry entry)
    {
      throw new System.NotImplementedException();
    }

    public override IDTableEntry GetID(string prefix, string key)
    {
      throw new System.NotImplementedException();
    }

    public override IDTableEntry[] GetKeys(string prefix, ID id)
    {
      throw new System.NotImplementedException();
    }

    public override void Remove(string prefix, string key)
    {
      throw new System.NotImplementedException();
    }

    public ThreadLocal<IDTableProvider> LocalProvider { get; private set; }

    public bool IsLocalProviderSet()
    {
      throw new System.NotImplementedException();
    }
  }
}