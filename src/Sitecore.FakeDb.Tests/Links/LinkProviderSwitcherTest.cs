namespace Sitecore.FakeDb.Tests.Links
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Common;
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

    [Theory, AutoData]
    public void SutSwitchesSwitcherSitecoreLinkProvider([Frozen]LinkProvider provider, LinkProviderSwitcher sut)
    {
      LinkManager.Provider.Name.Should().Be("switcher");
    }

    [Theory, AutoData]
    public void DisposeRestoresPreviousSitecoreLinkProvider([Frozen]LinkProvider provider, LinkProviderSwitcher sut)
    {
      sut.Dispose();
      LinkManager.Provider.Name.Should().Be("sitecore");
    }
  }
}