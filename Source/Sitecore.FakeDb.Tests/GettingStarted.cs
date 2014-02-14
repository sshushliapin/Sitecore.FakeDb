namespace Examples
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Analytics.Data.DataAccess;
  using Sitecore.ContentSearch.SearchTypes;
  using Sitecore.FakeDb;
  using Xunit;

  public class GettingStarted
  {
    [Fact]
    public void HowDoICreateASimpleItem()
    {
      using (var db = new Db { new DbItem("Home") { { "Title", "Welcome!" } } })
      {
        Sitecore.Data.Items.Item homeItem = db.GetItem("/sitecore/content/home");
        homeItem["Title"].Should().Be("Welcome!");
      }
    }

    [Fact]
    public void HowDoICreateAnItemHierarchy()
    {
      using (var db = new Db
                        {
                          new DbItem("Articles") { new DbItem("Getting Started"), new DbItem("Troubleshooting") }
                        })
      {
        db.GetItem("/sitecore/content/Articles").Should().NotBeNull();
        db.GetItem("/sitecore/content/Articles/Getting Started").Should().NotBeNull();
        db.GetItem("/sitecore/content/Articles/Troubleshooting").Should().NotBeNull();
      }
    }

    [Fact]
    public void HowDoICreateAMultilingualItem()
    {
      using (var db = new Db
                        {
                          new DbItem("home") { new DbField("Title") { { "en", "Hello!" }, { "da", "Hej!" } } }
                        })
      {
        db.GetItem("/sitecore/content/home", "en")["Title"].Should().Be("Hello!");
        db.GetItem("/sitecore/content/home", "da")["Title"].Should().Be("Hej!");
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

    [Fact]
    public void HowDoICreateAndConfigureAdvancedItemHierarchy()
    {
      using (Db db = new Db
        {
          new DbItem("home")
            {
              Fields = new DbFieldCollection { { "Title", "Welcome!" } },
              Children = new[]
                {
                  new DbItem("Articles")
                    {
                      new DbItem("Getting Started") { { "Description", "Articles helping to get started." } },
                      new DbItem("Troubleshooting") { { "Description", "Articles with solutions to common problems." } }
                    }
                }
            }
        })
      {
        Sitecore.Data.Items.Item homeItem = db.GetItem("/sitecore/content/home");
        homeItem["Title"].Should().Be("Welcome!");

        Sitecore.Data.Items.Item articles = db.GetItem("/sitecore/content/home/Articles");

        Sitecore.Data.Items.Item gettingStartedArticle = articles.Children["Getting Started"];
        gettingStartedArticle["Description"].Should().Be("Articles helping to get started.");

        Sitecore.Data.Items.Item troubleshootingArticle = articles.Children["Troubleshooting"];
        troubleshootingArticle["Description"].Should().Be("Articles with solutions to common problems.");
      }
    }

    [Fact]
    public void HowDoIMockContentSearchLogic()
    {
      // arrange
      try
      {
        var index = Substitute.For<Sitecore.ContentSearch.ISearchIndex>();
        Sitecore.ContentSearch.ContentSearchManager.SearchConfiguration.Indexes.Add("my_index", index);

        using (var db = new Db { new DbItem("home") })
        {
          var searchResultItem = Substitute.For<SearchResultItem>();
          searchResultItem.GetItem().Returns(db.GetItem("/sitecore/content/home"));
          index.CreateSearchContext().GetQueryable<SearchResultItem>().Returns((new[] { searchResultItem }).AsQueryable());

          // act
          Sitecore.Data.Items.Item result = index.CreateSearchContext().GetQueryable<SearchResultItem>().Single().GetItem();

          // assert
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
      // arrange
      var visitor = Substitute.For<Visitor>(Guid.NewGuid());

      // act
      using (new Sitecore.Common.Switcher<Visitor>(visitor))
      {
        // assert
        Sitecore.Analytics.Tracker.Visitor.Should().Be(visitor);
      }
    }

    [Fact]
    public void ShouldSetAsContextItem()
    {
      using (var db = new Db { new DbItem("Home") })
      {
        var item = db.GetItem("/sitecore/content/home");
        using (new Sitecore.Data.Items.ContextItemSwitcher(item))
        {
          Sitecore.Context.Item.Should().Be(item);
        }

        Sitecore.Context.Item.Should().BeNull();
      }
    }
  }
}