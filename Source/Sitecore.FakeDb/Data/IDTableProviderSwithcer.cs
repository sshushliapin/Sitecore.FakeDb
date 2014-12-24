namespace Sitecore.FakeDb.Data
{
  using Sitecore.Configuration;
  using Sitecore.Data.IDTables;

  public class IDTableProviderSwithcer : ThreadLocalProviderSwitcher<IDTableProvider>
  {
    public IDTableProviderSwithcer(IDTableProvider localProvider)
      : base((IThreadLocalProvider<IDTableProvider>)Factory.GetIDTable(), localProvider)
    {
    }
  }
}