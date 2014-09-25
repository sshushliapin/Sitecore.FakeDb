Sitecore FakeDb
===============

This is the unit testing framework for Sitecore that enables creation and manipulation of 
Sitecore content in memory. It is designed to minimize efforts for the test content 
initialization keeping focus on the minimal test data rather than comprehensive 
content tree representation.

FakeDb substitutes real Sitecore providers with fake "in-memory" providers to
avoid calls to database or configuration. The providers do not replicate real
Sitecore behavior, but are used mainly as stubs with minimal logic. They can be 
replaced with mocks in unit tests using the provider switchers.

Sitecore FakeDb does not set a goal to run the entire Sitecore instance in-memory. It
is a unit testing framework, which is not supposed to be used for integration
testing.

## Contents
- [Installation](#installation)
- [Content](#content)
  - [How to create a simple item](#how-to-create-a-simple-item)
  - [How to create an item under System](#how-to-create-an-item-under-system)
  - [How to create an item hierarchy](#how-to-create-an-item-hierarchy)
  - [How to create a multilingual item](#how-to-create-a-multilingual-item)
  - [How to create an item of specific template](#how-to-create-an-item-of-specific-template)
  - [How to create a versioned item](#how-to-create-a-versioned-item)
  - [How to create a template with standard values](#how-to-create-a-template-with-standard-values)
  - [How to create a template hierarchy](#how-to-create-a-template-hierarchy)
- [Security](#security)
  - [How to mock Authentication Provider](#how-to-mock-authentication-provider)
  - [How to mock Authorization Provider](#how-to-mock-authorization-provider)
  - [How to mock Role Provider](#how-to-mock-role-provider)
  - [How to mock Membership Provider](#how-to-mock-membership-provider)
  - [How to unit test item security with mocked provider](#how-to-unit-test-item-security-with-mocked-provider)
  - [How to unit test item security with fake provider](#how-to-unit-test-item-security-with-fake-provider)
  - [How to switch Context User](#how-to-switch-context-user)
  - [How to configure Item Access](#how-to-configure-item-access)
- [Pipelines](#pipelines)
  - [How to unit test a pipeline call](#how-to-unit-test-a-pipeline-call)
  - [How to configure a pipeline behaviour](#how-to-configure-a-pipeline-behaviour)
- [Configuration](#configuration)
  - [How to configure Settings](#how-to-configure-settings)
  - [Database lifetime configuration](#database-lifetime-configuration)
- [Miscellaneous](#miscellaneous)
  - [How to unit test localization](#how-to-unit-test-localization)
  - [How to work with Link Database](#how-to-work-with-link-database)
  - [How to mock Media Provider](#how-to-mock-media-provider)
  - [How to work with the Query API](#how-to-work-with-the-query-api)
  - [How to work with the Fast Query API](#how-to-work-with-the-fast-query-api)
  - [How to mock the content search logic](#how-to-mock-the-content-search-logic)
- [FakeDb NSubstitute](#fakedb-nsubstitute)

## <a id="installation"></a>Installation

Sitecore FakeDb is available on NuGet.
To install the framework:

1. Create a new Class Library project.
2. Add references to Sitecore.Kernel and Sitecore.Nexus assemblies.
3. Run the following command in the NuGet Package Manager Console:

  ```
  Install-Package Sitecore.FakeDb
  ```
      
To upgrade the framework run the following command in the NuGet Package Manager Console:

```
Update-Package Sitecore.FakeDb
```

> **Important:**

> You should always overwrite the App.config file if requested.

## <a id="content"></a>Content

### <a id="how-to-create-a-simple-item"></a>How to create a simple item

The code below creates a fake in-memory database with a single item Home that
contains the Title field with the 'Welcome!' value (the [xUnit](http://xunit.codeplex.com/)
unit testing framework is used):

``` csharp
[Fact]
public void HowToCreateSimpleItem()
{
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbItem("Home") { { "Title", "Welcome!" } }
    })
  {
    Sitecore.Data.Items.Item home = db.GetItem("/sitecore/content/home");
    Xunit.Assert.Equal("Welcome!", home["Title"]);
  }
}
```


> **Important:**

> You should always dispose the 'db' instance properly.
> By default Sitecore databases are singletones, so ignoring proper test data 
> cleaning may lead to unstable behavior in tests.


### <a id="how-to-create-an-item-under-system"></a>How to create an item under System

The code below sets `ParentID` explicitely:

```csharp
[Fact]
public void HowToCreateItemUnderSystem()
{
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbItem("Home") { ParentID = Sitecore.ItemIDs.SystemRoot }
    })
  {
    Sitecore.Data.Items.Item home = db.GetItem("/sitecore/system/home");
    Xunit.Assert.Equal("home", home.Key);
  }
}
```

### <a id="how-to-create-an-item-hierarchy"></a>How to create an item hierarchy

This code creates a root item Articles and two child items - Getting Started and Troubleshooting:

``` csharp
[Fact]
public void HowToCreateItemHierarchy()
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

    Xunit.Assert.NotNull(articles.Children["Getting Started"]);
    Xunit.Assert.NotNull(articles.Children["Troubleshooting"]);
  }
}
```    
 
### <a id="how-to-create-a-multilingual-item"></a>How to create a multilingual item

The following example demonstrates how to configure field values for different languages:

``` csharp
[Fact]
public void HowToCreateMultilingualItem()
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
    Xunit.Assert.Equal("Hello!", homeEn["Title"]);

    Sitecore.Data.Items.Item homeDa = db.GetItem("/sitecore/content/home", "da");
    Xunit.Assert.Equal("Hej!", homeDa["Title"]);
  }
}
```

### <a id="how-to-create-an-item-of-specific-template"></a>How to create an item on a specific template

In some cases you may want to create an item template first and only then add items based on this template.
It can be acheived using the following sample:

``` csharp
[Fact]
public void HowToCreateItemOnSpecificTemplate()
{
  Sitecore.Data.ID templateId = Sitecore.Data.ID.NewID;

  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbTemplate("products", templateId) { "Name" },
      new Sitecore.FakeDb.DbItem("Apple") { TemplateID = templateId }
    })
  {
    Sitecore.Data.Items.Item item = db.GetItem("/sitecore/content/apple");

    Xunit.Assert.Equal(templateId, item.TemplateID);
    Xunit.Assert.NotNull(item.Fields["Name"]);
  }
}
```

### <a id="how-to-create-a-versioned-item"></a>How to create a versioned item

``` csharp
[Fact]
public void HowToCreateVersionedItem()
{
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbItem("home")
        {
          new Sitecore.FakeDb.DbField("Title")
            {
              { "en", 1, "Hello!" },
              { "en", 2, "Welcome!" }
            }
        }
    })
  {
    Sitecore.Data.Items.Item home1 = db.GetItem("/sitecore/content/home", "en", 1);
    Xunit.Assert.Equal("Hello!", home1["Title"]);

    Sitecore.Data.Items.Item home2 = db.GetItem("/sitecore/content/home", "en", 2);
    Xunit.Assert.Equal("Welcome!", home2["Title"]);
  }
}
```

### <a id="how-to-create-a-template-with-standard-values"></a>How to create a template with standard values

``` csharp
[Fact]
public void HowToCreateTemplateWithStandardValues()
{
  var templateId = new Sitecore.Data.TemplateID(Sitecore.Data.ID.NewID);

  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      // create template with field Title and standard value $name
      new Sitecore.FakeDb.DbTemplate("Sample", templateId) { { "Title", "$name" } }
    })
  {
    // add item based on the template to the content root
    Sitecore.Data.Items.Item contentRoot = db.GetItem(Sitecore.ItemIDs.ContentRoot);
    Sitecore.Data.Items.Item item = contentRoot.Add("Home", templateId);

    // the Title field is set to 'Home'
    Xunit.Assert.Equal("Home", item["Title"]);
  }
}
```

### <a id="how-to-create-a-template-hierarchy"></a>How to create a template hierarchy

``` csharp
[Fact]
public void HowToCreateTemplateHierarchy()
{
  var baseTemplateIdOne = Sitecore.Data.ID.NewID;
  var baseTemplateIdTwo = Sitecore.Data.ID.NewID;
  var templateId = Sitecore.Data.ID.NewID;

  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
  {
    new Sitecore.FakeDb.DbTemplate("base one", baseTemplateIdOne),
    new Sitecore.FakeDb.DbTemplate("base two", baseTemplateIdTwo),
    new Sitecore.FakeDb.DbTemplate("Main", templateId)
      {
        BaseIDs = new[] { baseTemplateIdOne, baseTemplateIdTwo }
      }
  })
  {
    var template =
      Sitecore.Data.Managers.TemplateManager.GetTemplate(templateId, db.Database);

    Xunit.Assert.Contains(baseTemplateIdOne, template.BaseIDs);
    Xunit.Assert.Contains(baseTemplateIdTwo, template.BaseIDs);

    Xunit.Assert.True(template.InheritsFrom(baseTemplateIdOne));
    Xunit.Assert.True(template.InheritsFrom(baseTemplateIdTwo));
  }
}
```


## <a id="Security"></a>Security

By default, security allows to perform all the basic item operations without 
any additional configuration. For advanced scenarios where some security logic 
needs to be unit-tested, mocked providers and provider switchers can be used.

### <a id="how-to-mock-authentication-provider"></a>How to mock Authentication Provider

``` csharp
[Fact]
public void HowToMockAuthenticationProvider()
{
  // create and configure authentication provider mock
  var provider =
    Substitute.For<Sitecore.Security.Authentication.AuthenticationProvider>();

  provider.Login("John", true).Returns(true);

  // switch the authentication provider so the mocked version is used
  using (new Sitecore.Security.Authentication.AuthenticationSwitcher(provider))
  {
    // the authentication manager is called with expected parameters and returns True
    Xunit.Assert.True(
      Sitecore.Security.Authentication.AuthenticationManager.Login("John", true));

    // the authentication manager is called with wrong parameters and returns False
    Xunit.Assert.False(
      Sitecore.Security.Authentication.AuthenticationManager.Login("Robber", true));
  }
}
```

### <a id="how-to-mock-authorization-provider"></a>How to mock Authorization Provider

``` csharp
[Fact]
public void HowToMockAuthorizationProvider()
{
  // create sample user
  var user = Sitecore.Security.Accounts.User.FromName(@"extranet\John", true);

  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbItem("home")
    })
  {
    Sitecore.Data.Items.Item home = db.GetItem("/sitecore/content/home");

    // configure authorization provider mock to deny item read for the user
    var provider =
      Substitute.For<Sitecore.Security.AccessControl.AuthorizationProvider>();

    provider
      .GetAccess(home, user, Sitecore.Security.AccessControl.AccessRight.ItemRead)
      .Returns(new Sitecore.FakeDb.Security.AccessControl.DenyAccessResult());

    // switch the authorization provider
    using (new Sitecore.FakeDb.Security.AccessControl.AuthorizationSwitcher(provider))
    {
      // check the user cannot read the item
      bool canRead =
        Sitecore.Security.AccessControl.AuthorizationManager.IsAllowed(
          home,
          Sitecore.Security.AccessControl.AccessRight.ItemRead,
          user);

      Xunit.Assert.False(canRead);
    }
  }
}
```

### <a id="how-to-mock-role-provider"></a>How to mock Role Provider

``` csharp
[Fact]
public void HowToMockRoleProvider()
{
  // create and configure role provider mock
  string[] roles = { @"sitecore/Authors", @"sitecore/Editors" };

  var provider = Substitute.For<System.Web.Security.RoleProvider>();
  provider.GetAllRoles().Returns(roles);

  // switch the role provider so the mocked version is used
  using (new Sitecore.FakeDb.Security.Web.RoleProviderSwitcher(provider))
  {
    string[] resultRoles = System.Web.Security.Roles.GetAllRoles();

    Xunit.Assert.True(resultRoles.Contains(@"sitecore/Authors"));
    Xunit.Assert.True(resultRoles.Contains(@"sitecore/Editors"));
  }
}
```

### <a id="how-to-mock-membership-provider"></a>How to mock Membership Provider

``` csharp
[Fact]
public void HowToMockMembershipProvider()
{
  // create fake membership user
  var user = new Sitecore.FakeDb.Security.Accounts.FakeMembershipUser();

  // create membership provider mock
  var provider = NSubstitute.Substitute.For<System.Web.Security.MembershipProvider>();
  provider.GetUser(@"extranet\John", true).Returns(user);

  // switch the membership provider
  using (new Sitecore.FakeDb.Security.Web.MembershipSwitcher(provider))
  {
    // check if the user exists
    var exists = Sitecore.Security.Accounts.User.Exists(@"extranet\John");
    Xunit.Assert.True(exists);
  }
}
```

### <a id="how-to-unit-test-item-security-with-mocked-provider"></a>How to unit test item security with mocked provider

``` csharp
[Fact]
public void HowToUnitTestItemSecurityWithMockedProvider()
{
  // create sample item
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbItem("home")
    })
  {
    Sitecore.Data.Items.Item home = db.GetItem("/sitecore/content/home");

    // substitute the authorization provider
    var provider =
      Substitute.For<Sitecore.Security.AccessControl.AuthorizationProvider>();

    using (new Sitecore.FakeDb.Security.AccessControl.AuthorizationSwitcher(provider))
    {
      // call your business logic that changes the item security, e.g. denies Read
      // for Editors
      var account = Sitecore.Security.Accounts.Role.FromName(@"sitecore\Editors");
      var accessRight = Sitecore.Security.AccessControl.AccessRight.ItemRead;
      var propagationType = Sitecore.Security.AccessControl.PropagationType.Entity;
      var permission = Sitecore.Security.AccessControl.AccessPermission.Deny;

      Sitecore.Security.AccessControl.AccessRuleCollection rules =
        new Sitecore.Security.AccessControl.AccessRuleCollection
          {
            Sitecore.Security.AccessControl.AccessRule.Create
              (account, accessRight, propagationType, permission)
          };
      Sitecore.Security.AccessControl.AuthorizationManager.SetAccessRules(home, rules);

      // check the provider is called with proper arguments
      provider
        .Received()
        .SetAccessRules(
          home,
          NSubstitute.Arg.Is<Sitecore.Security.AccessControl.AccessRuleCollection>(
            r => r[0].Account.Name == @"sitecore\Editors"
              && r[0].AccessRight.Name == "item:read"
              && r[0].PropagationType.ToString() == "Entity"
              && r[0].SecurityPermission.ToString() == "DenyAccess"));
    }
  }
}
```

### <a id="how-to-unit-test-item-security-with-fake-provider"></a>How to unit test item security with fake provider

``` csharp
[Fact]
public void HowToUnitTestItemSecurityWithFakeProvider()
{
  // create sample item
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbItem("home")
    })
  {
    Sitecore.Data.Items.Item home = db.GetItem("/sitecore/content/home");

    // call your business logic that changes the item security, e.g. denies Read
    // for Editors
    var account = Sitecore.Security.Accounts.Role.FromName(@"sitecore\Editors");
    var accessRight = Sitecore.Security.AccessControl.AccessRight.ItemRead;
    var propagationType = Sitecore.Security.AccessControl.PropagationType.Entity;
    var permission = Sitecore.Security.AccessControl.AccessPermission.Deny;

    Sitecore.Security.AccessControl.AccessRuleCollection rules =
      new Sitecore.Security.AccessControl.AccessRuleCollection
          {
            Sitecore.Security.AccessControl.AccessRule.Create
              (account, accessRight, propagationType, permission)
          };
    Sitecore.Security.AccessControl.AuthorizationManager.SetAccessRules(home, rules);

    // check the account cannot read the item
    Xunit.Assert.False(home.Security.CanRead(account));
  }
}
```

### <a id="how-to-switch-context-user"></a>How to switch Context User

``` csharp
[Fact]
public void HowToSwitchContextUser()
{
  using (new Sitecore.Security.Accounts.UserSwitcher(@"extranet\John", true))
  {
    Xunit.Assert.Equal(@"extranet\John", Sitecore.Context.User.Name);
  }
}
```

### <a id="how-to-configure-item-access"></a>How to configure Item Access

The code below denies item read, so that the GetItem() method returns null: 

``` csharp
[Fact]
public void HowToConfigureItemAccess()
{
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
    {
      // set Access.CanRead to False
      new Sitecore.FakeDb.DbItem("home") { Access = { CanRead = false } }
    })
  {
    Sitecore.Data.Items.Item item = db.GetItem("/sitecore/content/home");

    // item is null because read is denied
    Xunit.Assert.Null(item);
  }
}
```


## <a id="pipelines"></a>Pipelines

### <a id="how-to-unit-test-a-pipeline-call"></a>How to unit test a pipeline call

Imagine you have a product repository. The repository should be able to get a 
product by id. The implementation of the repository is 'thin' and does nothing 
else than calling a corresponding pipeline with proper arguments. The following 
example shows how to unit test the pipeline calls (please note that the 
pipeline is not defined in the config file):

``` csharp
[Fact]
public void HowToUnitTestPipelineCall()
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

### <a id="how-to-configure-a-pipeline-behaviour"></a>How to configure a pipeline behaviour

The code sample above checks that the pipeline is called with proper arguments. 
The following scenario is used to validate the pipeline call results. 
In the code below we configure the pipeline's proressor behaviour to return an expected product only
if the product id id set to "1".

``` csharp
[Fact]
public void HowToConfigurePipelineBehaviour()
{
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
  {
    // create a product to get from the repository
    object expectedProduct = new object();
    string productId = "1";

    // configure Pipeline Watcher to expect a pipeline call where the args Custom Data
    // contains ProductId equals "1". Once the args received the pipeline result is set
    // to the Product Custom Data property
    db.PipelineWatcher
      .WhenCall("findProductById")
      .WithArgs(a => a.CustomData["ProductId"].Equals(productId))
      .Then(a => a.CustomData["Product"] = expectedProduct);

    // create a repository and call get product method
    ProductRepository repository = new ProductRepository();
    var actualProduct = repository.GetProductById(productId);

    // assert the received product is the same as the expected one
    Xunit.Assert.Equal(expectedProduct, actualProduct);
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


## <a id="configuration"></a>Configuration

### <a id="how-to-configure-settings"></a>How to configure Settings

In some cases you may prefer to use a setting instead of a dependency injected 
in your code via a constructor or property. The code below instantiates the new Db 
context and sets "MySetting" setting value to "1234". Please note that the 
setting is not defined explicitly in the App.config file, but nevertheless it 
is accessible using Sitecore.Configuration.Settings and can be used in the unit 
tests:

``` csharp
[Fact]
public void HowToConfigureSettings()
{
  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
  {
    // set the setting value in unit test using db instance
    db.Configuration.Settings["MySetting"] = "1234";

    // get the setting value in your code using regular Sitecore API
    var value = Sitecore.Configuration.Settings.GetSetting("MySetting");
    Xunit.Assert.Equal("1234", value);
  }
}
```

### <a id="database-lifetime-configuration"></a>Database lifetime configuration

By default Sitecore sets `singleInstance="true"` for all databases so that each 
of the three default databases behaves as singletones. This approach has a list 
of pros and cons; it is important to be avare about potential issues that may 
appear.

A single instance allows one to resolve a database in any place of code using 
Sitecore Factory. The same content is available no matter how many times the 
database has been resolved. The following code creates the Home item using the
simplified FakeDb API and then reads the item from the database resolved from
Factory:

``` csharp
[Fact]
public void HowToGetItemFromSitecoreDatabase()
{
  using (new Sitecore.FakeDb.Db
    {
      new Sitecore.FakeDb.DbItem("Home")
    })
  {
    Sitecore.Data.Database database = 
      Sitecore.Configuration.Factory.GetDatabase("master");
        
    Xunit.Assert.NotNull(database.GetItem("/sitecore/content/home"));
  }
}
```

It is important to remember that single instance objects may break unit tests 
isolation allowing the data from one test appear in another one. In order to 
minimize this negative impact, the Db context must always be disposed properly.

Sitecore FakeDb can deal with both single or transcient lifetime modes 
allowing developers to choose between usability or isolation.


## <a id="miscellaneous"></a>Miscellaneous    

### <a id="how-to-unit-test-localization"></a>How to unit test localization

FakeDb supports a simple localization mechanism. You can call the Translate.Text() or
Translate.TextByLanguage() method to get a 'translated' version of the original text.
The translated version gets the language name added to the initial phrase.

``` csharp
[Fact]
public void HowToUnitTestLocalization()
{
  // init languages
  Sitecore.Globalization.Language en = Sitecore.Globalization.Language.Parse("en");
  Sitecore.Globalization.Language da = Sitecore.Globalization.Language.Parse("da");

  const string Phrase = "Welcome!";

  // translate
  string enTranslation = Sitecore.Globalization.Translate.TextByLanguage(Phrase, en);
  string daTranslation = Sitecore.Globalization.Translate.TextByLanguage(Phrase, da);

  Xunit.Assert.Equal("en:Welcome!", enTranslation);
  Xunit.Assert.Equal("da:Welcome!", daTranslation);
}
```

### <a id="how-to-work-with-link-database"></a>How to work with Link Database

``` csharp
[Fact]
public void HowToWorkWithLinkDatabase()
{
  // arrange your database and items
  Sitecore.Data.ID sourceId = Sitecore.Data.ID.NewID;
  Sitecore.Data.ID aliasId = Sitecore.Data.ID.NewID;
  Sitecore.Data.ID linkedItemId = Sitecore.Data.ID.NewID;

  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
  {
    new Sitecore.FakeDb.DbItem("source", sourceId),
    new Sitecore.FakeDb.DbItem("clone"),
    new Sitecore.FakeDb.DbItem("alias", aliasId, Sitecore.TemplateIDs.Alias)
    {
      new Sitecore.FakeDb.DbField("Linked item", linkedItemId)
    }
  })
  {
    // arrange desired LinkDatabase behavior
    var behavior = Substitute.For<Sitecore.Links.LinkDatabase>();

    Sitecore.Data.Items.Item source = db.GetItem("/sitecore/content/source");
    Sitecore.Data.Items.Item alias = db.GetItem("/sitecore/content/alias");
    Sitecore.Data.Items.Item clone = db.GetItem("/sitecore/content/clone");

    string sourcePath = source.Paths.FullPath;
    behavior.GetReferrers(source).Returns(new[]
    {
      new Sitecore.Links.ItemLink(alias, linkedItemId, source, sourcePath),
      new Sitecore.Links.ItemLink(clone, Sitecore.FieldIDs.Source, source, sourcePath)
    });

    // link database is clean
    Xunit.Assert.Equal(Sitecore.Globals.LinkDatabase.GetReferrers(source).Count(), 0);

    using (new Sitecore.FakeDb.Links.LinkDatabaseSwitcher(behavior))
    {
      Sitecore.Links.ItemLink[] referrers =
        Sitecore.Globals.LinkDatabase.GetReferrers(source);

      Xunit.Assert.Equal(referrers.Count(), 2);
      Xunit.Assert.Equal(referrers.Count(r => r.SourceItemID == clone.ID
        && r.TargetItemID == source.ID), 1);
      Xunit.Assert.Equal(referrers.Count(r => r.SourceItemID == alias.ID
        && r.TargetItemID == source.ID), 1);
    }

    // link database is clean again
    Xunit.Assert.Equal(Sitecore.Globals.LinkDatabase.GetReferrers(source).Count(), 0);
  }
}
```

### <a id="how-to-mock-media-provider"></a>How to mock Media Provider

``` csharp
[Fact]
public void HowToMockMediaItemProvider()
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
      Xunit.Assert.Equal(MyImageUrl, mediaUrl);
    }
  }
}
```

### <a id="how-to-work-with-the-query-api"></a>How to work with the Query API

The `Query` API needs the `Context.Database` set, and the example below uses 
`DatabaseSwitcher` to do so:

```csharp
[Fact]
public void HowToWorkWithQueryApi()
{
  const string Query = "/sitecore/content/*[@@key = 'home']";

  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db 
    {
      new Sitecore.FakeDb.DbItem("home")
    })
  {
    Sitecore.Data.Items.Item[] result;
    using (new Sitecore.Data.DatabaseSwitcher(db.Database))
    {
      result = Sitecore.Data.Query.Query.SelectItems(Query);
    }

    Xunit.Assert.Equal(result.Count(), 1);
    Xunit.Assert.Equal(result[0].Key, "home");
  }
}
```

### <a id="how-to-work-with-the-fast-query-api"></a>How to work with the Fast Query API

```csharp
[Fact]
public void HowToWorkWithFastQueryApi()
{
  const string Query = "fast:/sitecore/content/*[@@key = 'home']";

  using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db 
    {
      new Sitecore.FakeDb.DbItem("home")
    })
  {
    Sitecore.Data.Items.Item homeItem = db.Database.SelectSingleItem(Query);

    Xunit.Assert.Equal(homeItem.Key, "home");
  }
}
```

> **Important:**

> Under the hood Sitecore Query is used. The Fast Query limitations are not applied to the result.

### <a id="how-to-mock-the-content-search-logic"></a>How to mock the content search logic

The example below creates and configures a content search index mock so that it returns the Home item:

``` csharp
[Fact]
public void HowToMockContentSearchLogic()
{
  try
  {
    var index = Substitute.For<Sitecore.ContentSearch.ISearchIndex>();
    Sitecore.ContentSearch.ContentSearchManager.SearchConfiguration.Indexes
      .Add("my_index", index);

    using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
      {
        new Sitecore.FakeDb.DbItem("home")
      })
    {
      var searchResultItem =
        Substitute.For<Sitecore.ContentSearch.SearchTypes.SearchResultItem>();

      searchResultItem
        .GetItem()
        .Returns(db.GetItem("/sitecore/content/home"));

      index
        .CreateSearchContext()
        .GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>()
        .Returns((new[] { searchResultItem }).AsQueryable());

      Sitecore.Data.Items.Item result =
        index
        .CreateSearchContext()
        .GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>()
        .Single()
        .GetItem();

      Xunit.Assert.Equal("/sitecore/content/home", result.Paths.FullPath);
    }
  }
  finally
  {
    Sitecore.ContentSearch.ContentSearchManager.SearchConfiguration.Indexes
      .Remove("my_index");
  }
}
```
    
## <a id="fakedb-nsubstitute"></a>FakeDb NSubstitute
Mocking is one of the fundamental things about unit testing. Mock objects 
allows to simulate abstraction behaviour keeping unit tests fast and 
isolated.

FakeDb allows to create [NSubstitute](http://nsubstitute.github.io/) mocks 
using Sitecore Factory.

In order to install the FakeDb.NSubstitute package run the following command 
in the NuGet Package Manager Console:

```
Install-Package Sitecore.FakeDb.NSubstitute
```

To instantiate a mock object, NSubstitute Factory should be used:

``` xml
<bucketManager enabled="true">
  <providers>
    <add name="mock" factory="nsubstitute"
      ref="Sitecore.Buckets.Managers.BucketProvider, Sitecore.Buckets" />
  </providers>
</bucketManager>
```

This configuration allows BucketManager to create a new mocked instance of the 
BucketProvider class.


> **Important:**

> BucketManager is a static class. It means that the mocked BucketProvider 
> instance can be shared between different unit tests, which may lead to 
> unstable behavior in tests.
