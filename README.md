Sitecore FakeDb
===============

A unit testing framework for Sitecore that enables creation and manipulation of Sitecore content in memory. Designed to minimize efforts for the test content initialization keeping focus on the minimal test data rather than comprehensive content tree representation.

### How do I install Sitecore FakeDb

The package is available on NuGet. To install the package, run the following command in the Package Manager Console:

      Install-Package Sitecore.FakeDb
      
When the package installation is done, go to the App.config file of the project you have the package installed and set path to the license.xml file in LicenseFile setting.


### How do I create a simple item

The code below creates a fake in-memory database with a single item Home that contains field Title with value 'Welcome!' ([xUnit](http://xunit.codeplex.com/) and [FluentAssertions](https://github.com/dennisdoomen/FluentAssertions) are used):

    [Fact]
    public void HowDoICreateASimpleItem()
    {
      using (var db = new Db { new DbItem("Home") { { "Title", "Welcome!" } } })
      {
        Sitecore.Data.Items.Item homeItem = db.GetItem("/sitecore/content/home");
        homeItem["Title"].Should().Be("Welcome!");
      }
    }

### How do I create an item hierarchy

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

### How do I create and configure an advanced item hierarchy

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
    
### How do I mock the content search logic
The example below creates and configure a content search index mock so that it returns Home item:

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
    
### How do I mock the Sitecore.Analytics.Tracker.Visitor

    [Fact]
    public void HowDoIMockTrackerVisitor()
    {
      // arrange
      var visitor = Substitute.For<Visitor>(Guid.NewGuid());

      // act
      using (new Switcher<Visitor>(visitor))
      {
        // assert
        Sitecore.Analytics.Tracker.Visitor.Should().Be(visitor);
      }
    
### What can I do with Sitecore FakeDb

Sitecore FakeDb allows you to perform the next manipulations in unit tests:

* create and edit items in memory
* read items by path or id
* access item children
* access item parent
* delete items
* initialize and configure 'master', 'web' or 'core' database
* mock the Content Search API
* mock Analytics Visitor
