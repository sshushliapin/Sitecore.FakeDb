namespace Sitecore.FakeDb.Data.IDTables
{
  using Sitecore.Configuration;
  using Sitecore.Data.IDTables;

  public class IDTableProviderSwitcher : ThreadLocalProviderSwitcher<IDTableProvider>
  {
    public IDTableProviderSwitcher(IDTableProvider localProvider)
      : base((IThreadLocalProvider<IDTableProvider>)Factory.GetIDTable(), localProvider)
    {
    }
  }
}