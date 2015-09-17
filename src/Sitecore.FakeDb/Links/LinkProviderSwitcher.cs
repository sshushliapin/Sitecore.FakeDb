namespace Sitecore.FakeDb.Links
{
  using Sitecore.Common;
  using Sitecore.Links;

  public class LinkProviderSwitcher : Switcher<LinkProvider>
  {
    private readonly Sitecore.Links.LinkProviderSwitcher providerSwitcher;

    public LinkProviderSwitcher(LinkProvider linkProviderToSwitchTo)
      : base(linkProviderToSwitchTo)
    {
      this.providerSwitcher = new Sitecore.Links.LinkProviderSwitcher("switcher");
    }

    public override void Dispose()
    {
      this.providerSwitcher.Dispose();
      base.Dispose();
    }
  }
}