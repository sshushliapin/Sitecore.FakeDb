namespace Sitecore.FakeDb.Sites
{
  using Sitecore.Sites;
  using Sitecore.Web;
  using StringDictionary = Sitecore.Collections.StringDictionary;

  public class FakeSiteContext : SiteContext
  {
    public FakeSiteContext(string name)
      : base(SiteInfo.Create(new StringDictionary { { "name", name } }))
    {
    }

    public FakeSiteContext(StringDictionary parameters)
      : base(SiteInfo.Create(parameters))
    {
    }
  }
}