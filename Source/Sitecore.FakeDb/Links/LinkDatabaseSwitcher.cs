namespace Sitecore.FakeDb.Links
{
  using Sitecore.Links;

  public class LinkDatabaseSwitcher : ProviderBehaviorSwitcher<LinkDatabase>
  {
    public LinkDatabaseSwitcher(LinkDatabase behavior)
      : base((FakeLinkDatabase)Globals.LinkDatabase, behavior)
    {
    }
  }
}