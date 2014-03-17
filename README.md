Sitecore FakeDb
===============

A unit testing framework for Sitecore that enables creation and manipulation of Sitecore content in memory. 
Designed to minimize efforts for the test content initialization keeping focus on the minimal test data rather 
than comprehensive content tree representation.

## Installation

Sitecore FakeDb is available on NuGet.
To install the framework:

1. Create a new Class Library project.
2. Add references to Sitecore.Kernel and Sitecore.Nexus assemblies.
3. Run the following command in the NuGet Package Manager Console:

      `Install-Package Sitecore.FakeDb`

4. Open App.config file added by the package and update path to the license.xml file using LicenseFile setting if necessary. By default the license file path is set to the root folder of the project:

      ``` xml
      <setting name="LicenseFile" value="..\..\license.xml" />
      ```

## How do I...
### How do I create a simple item

The code below creates a fake in-memory database with a single item Home that contains field Title with value 'Welcome!' 
([xUnit](http://xunit.codeplex.com/) unit testing framework is used):

``` csharp
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
```

### How do I create an item hierarchy

This code creates a root item Articles and two child items Getting Started and Troubleshooting:

``` csharp
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
```    
 
### How do I create a multilingual item

The next example demonstrates how to configure field values for different languages:

``` csharp
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
```

### How do I create an item of specific template

In some cases you may want to create an item template first and only then add items based on this template.
It can be acheived using the next sample:

``` csharp
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
```

## Security
### How do I configure item access

The code below denies item read, so that GetItem() method returns null: 

``` csharp
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
```

### How do I mock the authentication provider

The next example mocks the authentication provider and substitutes it so that authentication manager calls the mocked provider. [NSubstitute](http://nsubstitute.github.io/) mocking framework is used:

``` csharp
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
```

## Configuration
### How do I configure settings

In some cases you may prefer to use a setting instead of a dependency injected in your code via constructor or property. 
The code below instantiates new Db context and sets "MySetting" setting value to "1234". Please note that the setting is 
not defined explicitly in the App.config file, but nevertheless it is accessible using Sitecore.Configuration.Settings 
and can be used in unit tests:

``` csharp
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
```

## Miscellaneous    
### How do I mock the content search logic

The example below creates and configure a content search index mock so that it returns Home item:

``` csharp
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
```
    
### How do I mock the Sitecore.Analytics.Tracker.Visitor

``` csharp
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
```
