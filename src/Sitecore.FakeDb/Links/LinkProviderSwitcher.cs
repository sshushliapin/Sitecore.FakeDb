namespace Sitecore.FakeDb.Links
{
  using System;
  using Sitecore.Common;
  using Sitecore.Links;

  [Obsolete("This class is not supported starting from Sitecore 8.2.0. " +
            "Instead of switching the LinkProvider, please consider injecting " +
            "the Sitecore.Abstractions.BaseLinkManager class into your System " +
            "Under Test (SUT) via constructor.")]
  public class LinkProviderSwitcher : Switcher<LinkProvider>
  {
#if !SC72160123
    private readonly Sitecore.Links.LinkProviderSwitcher providerSwitcher;
#endif

    public LinkProviderSwitcher(LinkProvider linkProviderToSwitchTo)
      : base(linkProviderToSwitchTo)
    {
#if !SC72160123
      this.providerSwitcher = new Sitecore.Links.LinkProviderSwitcher("switcher");
#endif
    }

    public override void Dispose()
    {
#if !SC72160123
      this.providerSwitcher.Dispose();
#endif
      base.Dispose();
    }
  }
}