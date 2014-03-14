namespace Examples
{
  using System;
  using System.Linq;
  using FluentAssertions;
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
        homeItem["Title"].Should().Be("Welcome!");
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
        articles["Getting Started"].Should().NotBeNull();
        articles["Troubleshooting"].Should().NotBeNull();
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
        homeEn["Title"].Should().Be("Hello!");

        Sitecore.Data.Items.Item homeDa = db.GetItem("/sitecore/content/home", "da");
        homeDa["Title"].Should().Be("Hej!");
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
        item.TemplateID.Should().Be(templateId);
        item.Fields["Name"].Should().NotBeNull();
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
        item.Should().BeNull();
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
        Sitecore.Security.Authentication.AuthenticationManager.Login("John", false).Should().BeTrue();
      }
    }

    #endregion

    #region Configuration

    [Fact]
    public void HowDoIConfigureSettings()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
      {
        db.Configuration.Settings["MySetting"] = "1234";
        Sitecore.Configuration.Settings.GetSetting("MySetting").Should().Be("1234");
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

          result.Paths.FullPath.Should().Be("/sitecore/content/home");
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
      var visitor = Substitute.For<Sitecore.Analytics.Data.DataAccess.Visitor>(Guid.NewGuid());

      using (new Sitecore.Common.Switcher<Sitecore.Analytics.Data.DataAccess.Visitor>(visitor))
      {
        Sitecore.Analytics.Tracker.Visitor.Should().Be(visitor);
      }
    }

    #endregion
  }
}