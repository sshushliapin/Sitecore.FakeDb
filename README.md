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

### How do I create a versioned item

``` csharp
[Fact]
public void HowDoICreateAVersionedItem()
{
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbItem("home")
        {
          new Sitecore.FakeDb.DbField("Title") { { "en", 1, "Hello!" }, { "en", 2, "Welcome!" } }
        }
    })
  {
    Sitecore.Data.Items.Item homeV1 = db.GetItem("/sitecore/content/home", "en", 1);
    Assert.Equal("Hello!", homeV1["Title"]);

    Sitecore.Data.Items.Item homeV2 = db.GetItem("/sitecore/content/home", "en", 2);
    Assert.Equal("Welcome!", homeV2["Title"]);
  }
}
```

### How do I create a template with standard values

``` csharp
    [Fact]
    public void HowDoICreateATemplateWithStandardValues()
    {
      var templateId = Sitecore.Data.ID.NewID;

      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbTemplate("sample", templateId) { { "Title", "$name"} }
        })
      {
        var root = db.GetItem(Sitecore.ItemIDs.ContentRoot);
        var item = Sitecore.Data.Managers.ItemManager.CreateItem("Home", root, templateId);
        Assert.Equal("Home", item["Title"]);
      }
    }
```

### How do I work with LinkDatabase

``` csharp
[Fact]
public void HowDoIWorkWithLinkDatabase()
{
  // arrange your database and items

  Sitecore.Data.ID sourceId = Sitecore.Data.ID.NewID;

  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
  {
    new Sitecore.FakeDb.DbItem("source", sourceId),
    new Sitecore.FakeDb.DbItem("clone"),
    new Sitecore.FakeDb.DbItem("alias", Sitecore.Data.ID.NewID, Sitecore.TemplateIDs.Alias)
    {
      // not really needed but this is how aliases look in the real world
      new Sitecore.FakeDb.DbField("Linked item")
      {
        Type = "Link",
        Value = string.Format("<link linktype='internal' id='{0}' />", sourceId)
      }
    }
  })
  {
    // arrange desired LinkDatabase behavior
        
    var behavior = Substitute.For<Sitecore.Links.LinkDatabase>();

    Sitecore.Data.Items.Item source = db.GetItem("/sitecore/content/source");
    Sitecore.Data.Items.Item alias = db.GetItem("/sitecore/content/alias");
    Sitecore.Data.Items.Item clone = db.GetItem("/sitecore/content/clone");

    behavior.GetReferrers(source).Returns(new[]
    {
      new Sitecore.Links.ItemLink(alias, alias.Fields["Linked item"].ID, source, source.Paths.FullPath),
      new Sitecore.Links.ItemLink(clone, Sitecore.FieldIDs.Source, source, source.Paths.FullPath)
    });

    // act & assert

    // link database is clean
    Assert.Equal(Sitecore.Globals.LinkDatabase.GetReferrers(source).Count(), 0);
 
    using (new Sitecore.FakeDb.Links.LinkDatabaseSwitcher(behavior))
    {
      Sitecore.Links.ItemLink[] referrers = Sitecore.Globals.LinkDatabase.GetReferrers(source);

      Assert.Equal(referrers.Count(), 2);
      Assert.Equal(referrers.Count(r => r.SourceItemID == clone.ID && r.TargetItemID == source.ID), 1);
      Assert.Equal(referrers.Count(r => r.SourceItemID == alias.ID && r.TargetItemID == source.ID), 1);
    }
 
      // link database is clean again
    Assert.Equal(Sitecore.Globals.LinkDatabase.GetReferrers(source).Count(), 0);
  }
}
```

## Security
### How do I mock the authentication provider

``` csharp
[Fact]
public void HowDoIMockAuthenticationProvider()
{
  // create and configure authentication provider mock
  var provider = Substitute.For<Sitecore.Security.Authentication.AuthenticationProvider>();
  provider.Login("John", true).Returns(true);

  // switch the authentication provider so the mocked version is used
  using (new Sitecore.Security.Authentication.AuthenticationSwitcher(provider))
  {
    // the authentication manager is called with the expected parameters. It returns 'true'
    Assert.True(Sitecore.Security.Authentication.AuthenticationManager.Login("John", true));

    // the authentication manager is called with some unexpected parameters. It returns 'false'
    Assert.False(Sitecore.Security.Authentication.AuthenticationManager.Login("Robber", true));
  }
}
```

### How do I switch a context user

``` csharp
[Fact]
public void HowDoISwitchContextUser()
{
  using (new Sitecore.Security.Accounts.UserSwitcher("sitecore\\admin", true))
  {
    Assert.Equal("sitecore\\admin", Sitecore.Context.User.Name);
  }
}
```

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

## Pipelines
### How do I ensure the pipeline is called
Imagine you have a product repository. The repository should be able to get a product by id.
The implementation of the repository is 'thin' and does nothing than calling a corresponding pipeline with proper arguments.
The next example shows how to unit test the pipeline call (please note that the pipeline is not defined in the tests assembly config file):

``` csharp
[Fact]
public void HowDoIEnsureThePipelineIsCalled()
{
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
  {
    // configure a pipeline watcher to expect the "createProduct" pipeline call with
    // product name passed to the arguments custom data
    db.PipelineWatcher
      .Expects("createProduct", a => a.CustomData["ProductName"].Equals("MyProduct"));

    // create a repository and call the create product method
    var repository = new ProductRepository();
    repository.CreateProduct("MyProduct");

    // assert the expected pipeline is called and the product name is passed
    db.PipelineWatcher.EnsureExpectations();
  }
}

private partial class ProductRepository
{
  public void CreateProduct(string name)
  {
    var args = new Sitecore.Pipelines.PipelineArgs();
    args.CustomData["ProductName"] = name;

    Sitecore.Pipelines.CorePipeline.Run("createProduct", args);
  }
}
```

### How do I configure the pipeline behaviour
The code sample above checks that the pipeline is called with proper arguments. 
The next scenario would be to validate the pipeline call results. 
In the code below we configure pipeline proressor behaviour to return an expected product only
if the product id id set to "1".

``` csharp
[Fact]
public void HowDoIConfigureThePipelineBehaviour()
{
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
  {
    // create a product to get from the repository
    object expectedProduct = new object();

    // configure processing of the pipeline arguments. Will set the 'expectedProduct' instance 
    // to CustomData["Product"] property only when the CustomData["ProductId"] is "1"
    string productId = "1";

    // configure a pipeline watcher to expect a pipeline call where the args custom data contains
    // ProductId. Once the args received the pipeline result is set into Product custom data property
    db.PipelineWatcher
      .WhenCall("findProductById")
      .WithArgs(a => a.CustomData["ProductId"].Equals(productId))
      .Then(a => a.CustomData["Product"] = expectedProduct);

    // create a repository and call get product method
    ProductRepository repository = new ProductRepository();
    var actualProduct = repository.GetProductById(productId);

    // assert the received product is the same as the expected one
    Assert.Equal(expectedProduct, actualProduct);
  }
}

private partial class ProductRepository
{
  public object GetProductById(string id)
  {
    var args = new Sitecore.Pipelines.PipelineArgs();
    args.CustomData["ProductId"] = id;

    Sitecore.Pipelines.CorePipeline.Run("findProductById", args);

    return args.CustomData["Product"];
  }
}
```

## Globalization
### How do I translate texts
FakeDb supports simple localization mechanism. You can call Translate.Text() or
Translate.TextByLanguage() method to get a 'translated' version of the original text.
The translated version has got language name added to the initial phrase.

``` csharp
[Fact]
public void HowDoITranslateTexts()
{
  // init languages
  Sitecore.Globalization.Language enLang = Sitecore.Globalization.Language.Parse("en");
  Sitecore.Globalization.Language daLang = Sitecore.Globalization.Language.Parse("da");

  const string Phrase = "Welcome!";

  // translate
  string enTranslation = Sitecore.Globalization.Translate.TextByLanguage(Phrase, enLang);
  string daTranslation = Sitecore.Globalization.Translate.TextByLanguage(Phrase, daLang);

  Assert.Equal("en:Welcome!", enTranslation);
  Assert.Equal("da:Welcome!", daTranslation);
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

### Database lifetime configuration
By default Sitecore set `singleInstance="true"` for all databases so that each of the three default databases behaves as singletones. 
This approach has list of pros and cons; it is important to be avare about potential issues that may appear.

Single instance allows one to resolve a database in any place of code using Sitecore Factory. 
The same content is available no matter how many times the database has been resolved. 
The next code creates item Home using simplified FakeDb API and then reads the item from database resolved from Factory:

``` csharp
[Fact]
public void HowDoIGetItemFromSitecoreDatabase()
{
  using (new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbItem("Home")
    })
  {
    Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");
    Assert.NotNull(database.GetItem("/sitecore/content/home"));
  }
}
```
It is important to remember that single instance objects may break unit tests isolation allowing data from one test appear in another one. In order to minimize this negative impact Db context must always be disposed properly.

Sitecore FakeDb can deal with both single or transcient lifetime modes allowing developers to choose between usability or isolation.

## Mocks
Mocking is one of the fundamental things about unit testing. Mock objects allows to simulate an abstraction behaviour keeping unit tests fast and isolated.

FakeDb simplifies mocking of Sitecore components. There is a NuGet package 'Sitecore.FakeDb.NSubstitute' that allows to mock Authentication and Bucket providers 
using [NSubstitute](http://nsubstitute.github.io/) isolation framework.

Run the following command in the NuGet Package Manager Console:

`Install-Package Sitecore.FakeDb.NSubstitute`

Take a look on the examples below. AuthenticationManager.Provider is a mock. You may configure it's behaviour and then call the AuthenticationManager methods in your code.

``` csharp
[Fact]
public void HowDoIMockAuthenticationProvider()
{
  // the authentication provider is a mock created by NSubstitute;
  // the Login() method should return 'true' when it is called with parameters 'John' and 'true'
  Sitecore.Security.Authentication.AuthenticationManager.Provider.Login("John", true).Returns(true);

  // the authentication manager is called with the expected parameters. It returns 'true'
  Assert.True(Sitecore.Security.Authentication.AuthenticationManager.Login("John", true));

  // the authentication manager is called with some unexpected parameters. It returns 'false'
  Assert.False(Sitecore.Security.Authentication.AuthenticationManager.Login("Robber", true));
}

[Fact]
public void HowDoIMockBucketProvider()
{
  // act
  Sitecore.Buckets.Managers.BucketManager.AddSearchTabToItem(null);

  // assert
  Sitecore.Buckets.Managers.BucketManager.Provider.Received().AddSearchTabToItem(null);
}
```

## Media
### How do I mock the media item provider

``` csharp
[Fact]
public void HowDoIMockMediaItemProvider()
{
  const string MyImageUrl = "~/media/myimage.ashx";
  Sitecore.Data.ID mediaItemId = Sitecore.Data.ID.NewID;

  // create some media item. Location, fields and template are not important
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
  {
    new Sitecore.FakeDb.DbItem("my-image", mediaItemId)
  })
  {
    Sitecore.Data.Items.Item mediaItem = db.GetItem(mediaItemId);

    // create media provider mock and configure behaviour
    Sitecore.Resources.Media.MediaProvider mediaProvider =
      NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();
    
    mediaProvider
    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaItemId))
    .Returns(MyImageUrl);

    // substitute the original provider with the mocked one
    using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
    {
      string mediaUrl = Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem);
      Assert.Equal(MyImageUrl, mediaUrl);
    }
  }
}
```

## Miscellaneous    

### How do I work with the Query API?

The `Query` API needs `Context.Database` set and the example below uses `DatabaseSwitcher` to do so:

```csharp
[Fact]
public void HowDoIQorkWithQueryApi()
{
  using (var db = new Sitecore.FakeDb.Db { new Sitecore.FakeDb.DbItem("home") })
  {
    var query = "/sitecore/content/*[@@key = 'home']";

    Sitecore.Data.Items.Item[] result;
    using (new Sitecore.Data.DatabaseSwitcher(db.Database))
    {
      result = Sitecore.Data.Query.Query.SelectItems(query);
    }

    Assert.Equal(result.Count(), 1);
    Assert.Equal(result[0].Key, "home");
  }
}
```

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
