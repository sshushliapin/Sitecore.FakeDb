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
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("Home") { { "Title", "Welcome!" } }
        })
      {
        Sitecore.Data.Items.Item homeItem = db.GetItem("/sitecore/content/home");
        homeItem["Title"].Should().Be("Welcome!");
      }
    }

### How do I create an item hierarchy

This code creates a root item Articles and two child items Getting Started and Troubleshooting:

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
    
### How do I create a multilingual item

The next example demonstrates how to configure field values for different languages:

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

### How do I create an item of specific template

    [Fact]
    public void HowDoICreateAnItemOfSpecificTemplate()
    {
      // arrange
      Sitecore.Data.TemplateID templateId = new Sitecore.Data.TemplateID(Sitecore.Data.ID.NewID);

      // act
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbTemplate("products", templateId) { "Name" },
          new Sitecore.FakeDb.DbItem("Apple", templateId)
        })
      {
        // assert
        Sitecore.Data.Items.Item item = db.GetItem("/sitecore/content/apple");
        item.TemplateID.Should().Be(templateId.ID);
        item.Fields["Name"].Should().NotBeNull();
      }
    }

## Security
### How do I configure item access

    [Fact]
    public void HowDoIConfigureItemAccess()
    {
      // arrange & act
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home") { Access = { CanRead = false } }
        })
      {
        // assert
        Sitecore.Data.Items.Item item = db.GetItem("/sitecore/content/home");
        item.Should().BeNull();
      }
    }

### How do I mock the authentication provider

The next example mocks the authentication provider and substitutes it so that authentication manager calls the mocked provider. [NSubstitute](http://nsubstitute.github.io/) mocking framework is used:

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

## Miscellaneous    
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

        using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db { new Sitecore.FakeDb.DbItem("home") })
        {
          var searchResultItem = Substitute.For<Sitecore.ContentSearch.SearchTypes.SearchResultItem>();
          searchResultItem.GetItem().Returns(db.GetItem("/sitecore/content/home"));
          index.CreateSearchContext().GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>().Returns((new[] { searchResultItem }).AsQueryable());

          // act
          Sitecore.Data.Items.Item result = index.CreateSearchContext().GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>().Single().GetItem();

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
      var visitor = Substitute.For<Sitecore.Analytics.Data.DataAccess.Visitor>(Guid.NewGuid());

      // act
      using (new Sitecore.Common.Switcher<Sitecore.Analytics.Data.DataAccess.Visitor>(visitor))
      {
        // assert
        Sitecore.Analytics.Tracker.Visitor.Should().Be(visitor);
      }
    }