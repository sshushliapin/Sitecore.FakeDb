namespace Sitecore.FakeDb.Tests.Links
{
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Common;
  using Sitecore.Data.Items;
  using Sitecore.Links;
  using Xunit;
  using LinkProviderSwitcher = Sitecore.FakeDb.Links.LinkProviderSwitcher;

  public class LinkProviderSwitcherTest
  {
    [Theory, AutoData]
    public void SutIsSwitcher(LinkProviderSwitcher sut)
    {
      sut.Should().BeAssignableTo<Switcher<LinkProvider>>();
    }

    [Theory, AutoData]
    public void SutSwitchesSwitcherCurrentValue([Frozen]LinkProvider provider, LinkProviderSwitcher sut)
    {
      LinkProviderSwitcher.CurrentValue.Should().BeSameAs(provider);
    }

#if !SC72160123 && !SC82160729
    [Theory, AutoData]
    public void SutSwitchesSwitcherSitecoreLinkProvider([Frozen]LinkProvider provider, LinkProviderSwitcher sut)
    {
      LinkManager.Provider.Name.Should().Be("switcher");
    }
#endif

    [Theory, AutoData]
    public void DisposeRestoresPreviousSitecoreLinkProvider([Frozen]LinkProvider provider, LinkProviderSwitcher sut)
    {
      sut.Dispose();
      LinkManager.Provider.Name.Should().Be("sitecore");
    }

    [Theory, DefaultAutoData]
    public void SutSwitchesToMockedLinkProvider(Db db, Item item, [NoAutoProperties]UrlOptions options)
    {
      var provider = Substitute.For<LinkProvider>();
      provider.GetItemUrl(item, options).Returns("http://myawesomeurl.com");

      using (new LinkProviderSwitcher(provider))
      {
#if SC72160123
        LinkManager.Providers["switcher"].GetItemUrl(item, options).Should().Be("http://myawesomeurl.com");
#elif !SC82160729
        LinkManager.GetItemUrl(item, options).Should().Be("http://myawesomeurl.com");
#endif
      }
    }
  }
}