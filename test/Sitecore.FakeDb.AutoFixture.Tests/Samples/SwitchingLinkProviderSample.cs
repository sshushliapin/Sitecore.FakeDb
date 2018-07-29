#if !SC72160123 && !SC82160729 && !SC82161115 && !SC82161221
namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
    using System;
    using NSubstitute;
    using global::AutoFixture;
    using global::AutoFixture.AutoNSubstitute;
    using global::AutoFixture;
    using global::AutoFixture.AutoNSubstitute;
    using global::AutoFixture.Xunit2;
    using Sitecore.Data.Items;
    using Sitecore.FakeDb.AutoFixture;
    using Sitecore.Links;
    using Xunit;

    // NuGet packages required:
    // PM> Install-Package xunit
    // PM> Install-Package NSubstitute
    // PM> Install-Package AutoFixture.Xunit2
    // PM> Install-Package AutoFixture.AutoNSubstitute
    // PM> Install-Package Sitecore.FakeDb.AutoFixture
    [Obsolete("LinkProviderSwitcher is obsolete.")]
    public class SwitchingLinkProviderSample
    {
        [Theory, DefaultAutoData]
        public void SwitchLinkProvider([Substitute] LinkProvider provider,
            Item item, [Modest] UrlOptions options)
        {
            provider.GetItemUrl(item, options).Returns("http://mysite.com/myitem");
            using (new Sitecore.FakeDb.Links.LinkProviderSwitcher(provider))
            {
                Assert.Equal("http://mysite.com/myitem", LinkManager.GetItemUrl(item, options));
            }
        }

        private class DefaultAutoDataAttribute : AutoDataAttribute
        {
            public DefaultAutoDataAttribute()
                : base(new Fixture().Customize(new AutoNSubstituteCustomization())
                    .Customize(new AutoDbCustomization()))
            {
            }
        }
    }
}
#endif