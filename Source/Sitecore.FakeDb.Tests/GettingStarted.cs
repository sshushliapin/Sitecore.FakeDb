namespace Examples
{
  using System;
  using System.Linq;
  using NSubstitute;
  using Xunit;

  public class GettingStarted
  {
    #region Content

    [Fact]
    public void HowDoICreateASimpleItem()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("Home") { { "Title", "Welcome!" } }
        })
      {
        Sitecore.Data.Items.Item homeItem = db.GetItem("/sitecore/content/home");
        Assert.Equal("Welcome!", homeItem["Title"]);
      }
    }

    [Fact]
    public void HowDoICreateAnItemHierarchy()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("Articles")
            {
              new Sitecore.FakeDb.DbItem("Getting Started"),
              new Sitecore.FakeDb.DbItem("Troubleshooting")
            }
        })
      {
        Sitecore.Data.Items.Item articles = db.GetItem("/sitecore/content/Articles");
        Assert.NotNull(articles["Getting Started"]);
        Assert.NotNull(articles["Troubleshooting"]);
      }
    }

    [Fact]
    public void HowDoICreateAMultilingualItem()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home")
            {
              new Sitecore.FakeDb.DbField("Title") { { "en", "Hello!" }, { "da", "Hej!" } }
            }
        })
      {
        Sitecore.Data.Items.Item homeEn = db.GetItem("/sitecore/content/home", "en");
        Assert.Equal("Hello!", homeEn["Title"]);

        Sitecore.Data.Items.Item homeDa = db.GetItem("/sitecore/content/home", "da");
        Assert.Equal("Hej!", homeDa["Title"]);
      }
    }

    [Fact]
    public void HowDoICreateAnItemOfSpecificTemplate()
    {
      Sitecore.Data.ID templateId = Sitecore.Data.ID.NewID;

      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbTemplate("products", templateId) { "Name" },
          new Sitecore.FakeDb.DbItem("Apple") { TemplateID = templateId }
        })
      {
        Sitecore.Data.Items.Item item = db.GetItem("/sitecore/content/apple");
        Assert.Equal(templateId, item.TemplateID);
        Assert.NotNull(item.Fields["Name"]);
      }
    }

    #endregion

    #region Security

    [Fact]
    public void HowDoIConfigureItemAccess()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home") { Access = { CanRead = false } }
        })
      {
        Sitecore.Data.Items.Item item = db.GetItem("/sitecore/content/home");

        // item is null because read is denied
        Assert.Null(item);
      }
    }

    [Fact]
    public void HowDoIMockAuthenticationProvider()
    {
      // create mock and configure behaviour of the authentication provider
      var provider = Substitute.For<Sitecore.Security.Authentication.AuthenticationProvider>();
      provider.Login("John", false).Returns(true);

      // substitute authentication provider with mock
      using (new Sitecore.Common.Switcher<Sitecore.Security.Authentication.AuthenticationProvider>(provider))
      {
        // use authentication manager in your code
        bool isLoggedIn = Sitecore.Security.Authentication.AuthenticationManager.Login("John", false);
        Assert.True(isLoggedIn);
      }
    }

    #endregion

    #region Configuration

    [Fact]
    public void HowDoIConfigureSettings()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
      {
        // set the setting value in unit test using db instance
        db.Configuration.Settings["MySetting"] = "1234";

        // get the setting value in your code using regular Sitecore API
        var value = Sitecore.Configuration.Settings.GetSetting("MySetting");
        Assert.Equal("1234", value);
      }
    }

    #endregion

    #region Miscellaneous

    [Fact]
    public void HowDoIMockContentSearchLogic()
    {
      try
      {
        var index = Substitute.For<Sitecore.ContentSearch.ISearchIndex>();
        Sitecore.ContentSearch.ContentSearchManager.SearchConfiguration.Indexes.Add("my_index", index);

        using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db { new Sitecore.FakeDb.DbItem("home") })
        {
          var searchResultItem = Substitute.For<Sitecore.ContentSearch.SearchTypes.SearchResultItem>();
          searchResultItem.GetItem().Returns(db.GetItem("/sitecore/content/home"));
          index.CreateSearchContext().GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>().Returns((new[] { searchResultItem }).AsQueryable());

          Sitecore.Data.Items.Item result = index.CreateSearchContext().GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>().Single().GetItem();

          Assert.Equal("/sitecore/content/home", result.Paths.FullPath);
        }
      }
      finally
      {
        Sitecore.ContentSearch.ContentSearchManager.SearchConfiguration.Indexes.Remove("my_index");
      }
    }

    [Fact]
    public void HowDoIMockTrackerVisitor()
    {
      // create a visitor mock
      var visitorMock = Substitute.For<Sitecore.Analytics.Data.DataAccess.Visitor>(Guid.NewGuid());

      // inject the visitor mock into Analytics Tracker
      using (new Sitecore.Common.Switcher<Sitecore.Analytics.Data.DataAccess.Visitor>(visitorMock))
      {
        // the mocked visitor instance is now available via Analytics.Tracker.Visitor property
        var currentVisitor = Sitecore.Analytics.Tracker.Visitor;
        Assert.Equal(visitorMock, currentVisitor);
      }
    }

    #endregion
  }
}