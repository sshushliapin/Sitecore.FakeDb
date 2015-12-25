namespace Sitecore.FakeDb.Links
{
  using Sitecore.Common;
  using Sitecore.Links;

  public class LinkProviderSwitcher : Switcher<LinkProvider>
  {
#if SC80 || SC81 || SC811
    private readonly Sitecore.Links.LinkProviderSwitcher providerSwitcher;
#endif

    public LinkProviderSwitcher(LinkProvider linkProviderToSwitchTo)
      : base(linkProviderToSwitchTo)
    {
#if SC80 || SC81 || SC811
      this.providerSwitcher = new Sitecore.Links.LinkProviderSwitcher("switcher");
#endif
    }

    public override void Dispose()
    {
#if SC80 || SC81 || SC811
      this.providerSwitcher.Dispose();
#endif
      base.Dispose();
    }
  }
}