namespace Sitecore.FakeDb.Links
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using Sitecore.Common;
    using Sitecore.Data.Items;
    using Sitecore.Links;
    using Sitecore.Links.UrlBuilders;
    using Sitecore.Web;

    [Obsolete("This class is not supported starting from Sitecore 8.2.0. " +
              "Instead of switching the LinkProvider, please consider injecting " +
              "the Sitecore.Abstractions.BaseLinkManager class into your System " +
              "Under Test (SUT) via constructor.")]
    public class SwitchingLinkProvider : LinkProvider
    {
        public LinkProvider CurrentProvider
        {
            get { return Switcher<LinkProvider>.CurrentValue; }
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

        [Obsolete("Please use GetDefaultUrlBuilderOptions() instead.")]
        public override UrlOptions GetDefaultUrlOptions()
        {
            var current = this.CurrentProvider;
            return current != null ? current.GetDefaultUrlOptions() : base.GetDefaultUrlOptions();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.Initialize(name, config);
            }
            else
            {
                base.Initialize(name, config);
            }
        }

        public override string GetDynamicUrl(Item item, LinkUrlOptions options)
        {
            var current = this.CurrentProvider;
            return current != null ? current.GetDynamicUrl(item, options) : base.GetDynamicUrl(item, options);
        }

        public override string GetItemUrl(Item item, ItemUrlBuilderOptions options)
        {
            var current = this.CurrentProvider;
            return current != null ? current.GetItemUrl(item, options) : base.GetItemUrl(item, options);
        }

        [Obsolete("Please use GetItemUrl(Item, ItemUrlBuilderOptions) instead.")]
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            var current = this.CurrentProvider;
            return current != null ? current.GetItemUrl(item, options) : base.GetItemUrl(item, options);
        }

        public override bool IsDynamicLink(string linkText)
        {
            var current = this.CurrentProvider;
            return current != null ? current.IsDynamicLink(linkText) : base.IsDynamicLink(linkText);
        }

        public override DynamicLink ParseDynamicLink(string linkText)
        {
            var current = this.CurrentProvider;
            return current != null ? current.ParseDynamicLink(linkText) : base.ParseDynamicLink(linkText);
        }

        public override RequestUrl ParseRequestUrl(HttpRequestBase request)
        {
            var current = this.CurrentProvider;
            return current != null ? current.ParseRequestUrl(request) : base.ParseRequestUrl(request);
        }

        [Obsolete("Please use IItemBasedSiteResolver instead.")]
        public override SiteInfo ResolveTargetSite(Item item)
        {
            var current = this.CurrentProvider;
            return current != null ? current.ResolveTargetSite(item) : base.ResolveTargetSite(item);
        }

        public override string ExpandDynamicLinks(string text, bool resolveSites)
        {
            var current = this.CurrentProvider;
            return current != null ? current.ExpandDynamicLinks(text, resolveSites) : base.ExpandDynamicLinks(text, resolveSites);
        }
    }
}
