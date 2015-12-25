namespace Sitecore.FakeDb.Tests.Links
{
  using System.Collections.Specialized;
  using System.Web;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.AutoNSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Common;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Links;
  using Sitecore.Links;
  using Sitecore.Web;
  using Xunit;
  using StringDictionary = Sitecore.Collections.StringDictionary;

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

    [Theory, SwitchingAutoData]
    public void SutUsesDefaultNameIfCurrentProviderSet(SwitchingLinkProvider sut, [Substitute] LinkProvider current)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.Name.Should().BeNull();
      }
    }

    [Theory, SwitchingAutoData]
    public void SutCallsCurrentProviderProperties(SwitchingLinkProvider sut, [Substitute]LinkProvider current)
    {
      using (new Switcher<LinkProvider>(current))
      {
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

    [Theory, SwitchingAutoData]
    public void SutCallsBaseProviderPropertiesIfCurrentNorSet(SwitchingLinkProvider sut)
    {
      sut.Name.Should().BeNull();
      sut.AddAspxExtension.Should().BeFalse();
      sut.AlwaysIncludeServerUrl.Should().BeFalse();
      sut.Description.Should().BeNull();
      sut.EncodeNames.Should().BeFalse();
      sut.LanguageEmbedding.Should().Be(LanguageEmbedding.AsNeeded);
      sut.LanguageLocation.Should().Be(LanguageLocation.FilePath);
      sut.LowercaseUrls.Should().BeFalse();
      sut.ShortenUrls.Should().BeFalse();
      sut.UseDisplayName.Should().BeFalse();
    }

    [Theory, SwitchingAutoData]
    public void GetDefaultUrlOptionsCallsCurrentProvider(SwitchingLinkProvider sut, [Substitute] LinkProvider current)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.GetDefaultUrlOptions().Should().BeSameAs(current.GetDefaultUrlOptions());
      }
    }

    [Theory, SwitchingAutoData]
    public void GetDefaultUrlOptionsCallsBaseProviderIfCurrentNotSet(SwitchingLinkProvider sut)
    {
      sut.GetDefaultUrlOptions().Should().NotBeNull();
    }

    [Theory, SwitchingAutoData]
    public void GetDynamicUrlCallsCurrentProvider(SwitchingLinkProvider sut, [Substitute] LinkProvider current, Item item, LinkUrlOptions options)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.GetDynamicUrl(item, options).Should().BeSameAs(current.GetDynamicUrl(item, options));
      }
    }

    [Theory, SwitchingAutoData]
    public void GetDynamicUrlCallsCallsBaseProviderIfCurrentNotSet(SwitchingLinkProvider sut, Item item, LinkUrlOptions options)
    {
      sut.GetDynamicUrl(item, options).Should().NotBeEmpty();
    }

    [Theory, SwitchingAutoData]
    public void GetItemUrlCallsCurrentProvider(SwitchingLinkProvider sut, [Substitute] LinkProvider current, Item item, UrlOptions options)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.GetItemUrl(item, options).Should().BeSameAs(current.GetItemUrl(item, options));
      }
    }

    [Theory, SwitchingAutoData]
    public void GetItemUrlOptionsCallsBaseProviderIfCurrentNotSet(SwitchingLinkProvider sut, Item item, UrlOptions options)
    {
      using (new Db())
      {
        sut.GetItemUrl(item, options).Should().NotBeNull();
      }
    }

    [Theory, SwitchingAutoData]
    public void InitializeCallsBaseProviderIfCurrentSet(SwitchingLinkProvider sut, [Substitute] LinkProvider current, string name, NameValueCollection config)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.Initialize(name, config);
        current.DidNotReceiveWithAnyArgs().Initialize(name, config);
      }
    }

    [Theory, SwitchingAutoData]
    public void InitializeCallsBaseProviderIfCurrentNotSet(SwitchingLinkProvider sut, string name, NameValueCollection config)
    {
      sut.Initialize(name, config);
    }

    [Theory, SwitchingAutoData]
    public void IsDynamicLinkCallsCurrentProvider(SwitchingLinkProvider sut, [Substitute] LinkProvider current, string linkText)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.IsDynamicLink(linkText).Should().Be(current.IsDynamicLink(linkText));
      }
    }

    [Theory, SwitchingAutoData]
    public void IsDynamicLinkCallsBaseProviderIfCurrentNotSet(SwitchingLinkProvider sut, string linkText)
    {
      sut.IsDynamicLink(linkText).Should().BeFalse();
    }

    [Theory, SwitchingAutoData]
    public void ParseDynamicLinkCallsCurrentProvider(SwitchingLinkProvider sut, [Substitute] LinkProvider current, ID id)
    {
      using (new Switcher<LinkProvider>(current))
      {
        var linkText = "~/link.aspx?_id=" + id;
        sut.ParseDynamicLink(linkText).Should().Be(current.ParseDynamicLink(linkText));
      }
    }

    [Theory, SwitchingAutoData]
    public void ParseDynamicLinkCallsBaseProviderIfCurrentNotSet(SwitchingLinkProvider sut, ID id)
    {
      var linkText = "~/link.aspx?_id=" + id;
      sut.ParseDynamicLink(linkText).Should().NotBeNull();
    }

    [Theory, SwitchingAutoData]
    public void ParseRequestUrlCallsCurrentProvider(SwitchingLinkProvider sut, [Substitute] LinkProvider current, HttpRequest request)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.ParseRequestUrl(request).Should().Be(current.ParseRequestUrl(request));
      }
    }

    [Theory, SwitchingAutoData]
    public void ParseRequestUrlCallsBaseProviderIfCurrentNotSet(SwitchingLinkProvider sut, HttpRequest request)
    {
      sut.ParseRequestUrl(request).Should().NotBeNull();
    }

#if SC81 || SC811
    [Theory, SwitchingAutoData]
    public void ResolveTargetSiteCallsCurrentProvider(SwitchingLinkProvider sut, [Substitute] LinkProvider current, Item item)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.ResolveTargetSite(item).Should().Be(current.ResolveTargetSite(item));
      }
    }

    [Theory, SwitchingAutoData]
    public void ResolveTargetSiteCallsBaseProviderIfCurrentNotSet(SwitchingLinkProvider sut, HttpRequest request, Item item)
    {
      sut.ResolveTargetSite(item);
    }
#endif

    [Theory, SwitchingAutoData]
    public void ExpandDynamicLinksCallsCurrentProvider(SwitchingLinkProvider sut, [Substitute] LinkProvider current, string text, bool resolveSite)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.ExpandDynamicLinks(text, resolveSite).Should().Be(current.ExpandDynamicLinks(text, resolveSite));
      }
    }

    [Theory, SwitchingAutoData]
    public void ExpandDynamicLinksCallsBaseProviderIfCurrentNotSet(SwitchingLinkProvider sut, HttpRequest request, string text, bool resolveSites)
    {
      sut.ExpandDynamicLinks(text, resolveSites);
    }

    private class SwitchingAutoDataAttribute : DefaultAutoDataAttribute
    {
      public SwitchingAutoDataAttribute()
      {
        this.Fixture.Customize(new AutoNSubstituteCustomization())
                    .Customize(new AutoConfiguredNSubstituteCustomization());

        this.Fixture.Register(() => LanguageEmbedding.Never);
        this.Fixture.Register(() => LanguageLocation.QueryString);
        this.Fixture.Register(() => new HttpRequest("default.aspx", "http://google.com", null));
        this.Fixture.Register(() => new SiteInfo(new StringDictionary()));
        this.Fixture.Customize<UrlOptions>(x => x.OmitAutoProperties());
      }
    }
  }
}