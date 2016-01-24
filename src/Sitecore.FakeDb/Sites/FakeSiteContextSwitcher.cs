using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.FakeDb.Sites
{
    using System.Collections.Specialized;
    using Sitecore.Sites;
    using Sitecore.Web;
    using StringDictionary = Sitecore.Collections.StringDictionary;

    /// <summary>
    /// Switches The Site Context and sync context with Factory.Sites 
    /// </summary>
    public class FakeSiteContextSwitcher: SiteContextSwitcher
    {
        private readonly List<SiteInfo> _pocket = new List<SiteInfo>(); 

        public FakeSiteContextSwitcher(SiteContext site) : base(site)
        {
            if (site == null) throw new ArgumentNullException(nameof(site));

            _pocket.AddRange(SiteContextFactory.Sites);
            //SiteContextFactory stores cashed data in _searchTable so clearing .Sites is not enough
            SiteContextFactory.Reset();
            //Returning back all sites except Context to avoid duplication
            AddOnlyNew(SiteContextFactory.Sites, _pocket.Where(t => t.Name != site.Name));

            SiteContextFactory.Sites.Add(new SiteInfo(new StringDictionary(ToDictionary(site.Properties))));
        }

        private IDictionary<string, string> ToDictionary(NameValueCollection col)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var k in col.AllKeys)
            {
                dict.Add(k, col[k]);
            }
            return dict;
        }

        private void AddOnlyNew(List<SiteInfo> currentSites, IEnumerable<SiteInfo> update)
        {
            currentSites.AddRange(update.Where(t => currentSites.All(s => s.Name != t.Name)));
        }

        public override void Dispose()
        {
            SiteContextFactory.Reset();
            AddOnlyNew(SiteContextFactory.Sites, _pocket);
            base.Dispose();
        }
    }
}
