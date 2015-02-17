namespace Sitecore.FakeDb.Links
{
  using Sitecore.Links;

  public class LinkDatabaseSwitcher : ThreadLocalProviderSwitcher<LinkDatabase>
  {
    public LinkDatabaseSwitcher(LinkDatabase localProvider)
      : base((IThreadLocalProvider<LinkDatabase>)Globals.LinkDatabase, localProvider)
    {
    }
  }
}