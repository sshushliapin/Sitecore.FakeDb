namespace Sitecore.FakeDb.Tests.Links
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture.AutoNSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Common;
  using Sitecore.Links;
  using Xunit;

  public class SwitchingLinkProviderTest
  {
    [Theory, AutoData]
    public void SutIsLinkProvider(SwitchingLinkProvider sut)
    {
      sut.Should().BeAssignableTo<LinkProvider>();
    }

    [Theory, AutoData]
    public void SutGetCurrentProvider(SwitchingLinkProvider sut, Switcher<LinkProvider> switcher)
    {
      sut.CurrentProvider.Should().BeSameAs(Switcher<LinkProvider>.CurrentValue);
    }

    [Theory, DefaultAutoData]
    public void SutCallsCurrentProviderProperties(SwitchingLinkProvider sut, [Substitute]LinkProvider current)
    {
      // if add LanguageEmbedding parameter, test fails (which is correct). Investigate why does not fail now.

      using (new Switcher<LinkProvider>(current))
      {
        sut.Name.Should().Be(current.Name);
        sut.AddAspxExtension.Should().Be(current.AddAspxExtension);
        sut.AlwaysIncludeServerUrl.Should().Be(current.AlwaysIncludeServerUrl);
        sut.Description.Should().Be(current.Description);
        sut.EncodeNames.Should().Be(current.EncodeNames);
        sut.LanguageEmbedding.Should().Be(current.LanguageEmbedding);
        sut.LanguageLocation.Should().Be(current.LanguageLocation);
        sut.LowercaseUrls.Should().Be(current.LowercaseUrls);
        sut.ShortenUrls.Should().Be(current.ShortenUrls);
        sut.UseDisplayName.Should().Be(current.UseDisplayName);
      }
    }

    [Theory, DefaultAutoData]
    public void SutCallsCurrentProviderMethods(SwitchingLinkProvider sut, [Substitute]LinkProvider current)
    {
      using (new Switcher<LinkProvider>(current))
      {
      }
    }
  }

  public class SwitchingLinkProvider : LinkProvider
  {
    public LinkProvider CurrentProvider
    {
      get { return Switcher<LinkProvider>.CurrentValue; }
    }

    public override string Name
    {
      get { return this.CurrentProvider.Name; }
    }

    public override bool AddAspxExtension
    {
      get { return this.CurrentProvider.AddAspxExtension; }
    }

    public override bool AlwaysIncludeServerUrl
    {
      get { return this.CurrentProvider.AlwaysIncludeServerUrl; }
    }

    public override string Description
    {
      get { return this.CurrentProvider.Description; }
    }

    public override bool EncodeNames
    {
      get { return this.CurrentProvider.EncodeNames; }
    }

    public override bool LowercaseUrls
    {
      get { return this.CurrentProvider.LowercaseUrls; }
    }

    public override bool ShortenUrls
    {
      get { return this.CurrentProvider.ShortenUrls; }
    }

    public override bool UseDisplayName
    {
      get { return this.CurrentProvider.UseDisplayName; }
    }
  }
}