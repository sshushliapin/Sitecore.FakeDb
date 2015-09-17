namespace Sitecore.FakeDb.Tests.Links
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture;
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

    [Theory, AutoSwitchingData]
    public void SutCallsCurrentProviderProperties(SwitchingLinkProvider sut, [Substitute]LinkProvider current)
    {
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

    [Theory, AutoSwitchingData]
    public void SutCallsBaseProviderPropertiesIfNoCurrentSet(SwitchingLinkProvider sut)
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


    [Theory, AutoSwitchingData]
    public void SutCallsCurrentProviderMethods(SwitchingLinkProvider sut, [Substitute]LinkProvider current)
    {
      using (new Switcher<LinkProvider>(current))
      {
        sut.GetDefaultUrlOptions().Should().BeSameAs(current.GetDefaultUrlOptions());
      }
    }

    public class AutoSwitchingDataAttribute : DefaultAutoDataAttribute
    {
      public AutoSwitchingDataAttribute()
      {
        this.Fixture.Customize(new AutoNSubstituteCustomization())
                    .Customize(new AutoConfiguredNSubstituteCustomization());

        this.Fixture.Register(() => LanguageEmbedding.Never);
        this.Fixture.Register(() => LanguageLocation.QueryString);
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
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.Name : base.Name;
      }
    }

    public override bool AddAspxExtension
    {
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.AddAspxExtension : base.AddAspxExtension;
      }
    }

    public override bool AlwaysIncludeServerUrl
    {
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.AlwaysIncludeServerUrl : base.AlwaysIncludeServerUrl;
      }
    }

    public override string Description
    {
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.Description : base.Description;
      }
    }

    public override bool EncodeNames
    {
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.EncodeNames : base.EncodeNames;
      }
    }

    public override LanguageEmbedding LanguageEmbedding
    {
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.LanguageEmbedding : base.LanguageEmbedding;
      }
    }

    public override LanguageLocation LanguageLocation
    {
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.LanguageLocation : base.LanguageLocation;
      }
    }

    public override bool LowercaseUrls
    {
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.LowercaseUrls : base.LowercaseUrls;
      }
    }

    public override bool ShortenUrls
    {
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.ShortenUrls : base.ShortenUrls;
      }
    }

    public override bool UseDisplayName
    {
      get
      {
        var current = this.CurrentProvider;
        return current != null ? current.UseDisplayName : base.UseDisplayName;
      }
    }
  }
}