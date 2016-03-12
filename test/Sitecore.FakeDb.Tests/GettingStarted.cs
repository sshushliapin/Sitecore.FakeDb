namespace Examples
{
  using System.Linq;
  using NSubstitute;
  using Sitecore.Configuration;
  using Xunit;

  public class GettingStarted
  {
    #region Content

    /// <summary>
    /// The code below creates a fake in-memory database with a single item Home that
    /// contains field Title with value 'Welcome!' (xUnit unit testing framework is used):
    /// </summary>
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

    [Fact]
    public void HowToCreateItemWithSpecificTemplate()
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

    [Fact]
    public void HowToCreateLinkFieldUsingRawValue()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home")
            {
              { "link", "<link linktype=\"external\" url=\"http://google.com\" />" }
            }
        })
      {
        var item = db.GetItem("/sitecore/content/home");

        var linkField = (Sitecore.Data.Fields.LinkField)item.Fields["link"];

        Xunit.Assert.Equal("external", linkField.LinkType);
        Xunit.Assert.Equal("http://google.com", linkField.Url);
      }
    }

    [Fact]
    public void HowToCreateLinkFieldUsingDbLinkField()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home")
            {
              new Sitecore.FakeDb.DbLinkField("link")
                {
                  LinkType = "external", Url = "http://google.com"
                }
            }
        })
      {
        var item = db.GetItem("/sitecore/content/home");

        var linkField = (Sitecore.Data.Fields.LinkField)item.Fields["link"];

        Xunit.Assert.Equal("external", linkField.LinkType);
        Xunit.Assert.Equal("http://google.com", linkField.Url);
      }
    }

    #endregion

    #region Security

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

    [Fact]
    public void HowToMockRolesInRolesProvider()
    {
      // create and configure roles-in-roles provider mock
      var role = Sitecore.Security.Accounts.Role.FromName(@"sitecore/Editors");
      var user = Sitecore.Security.Accounts.User.FromName(@"sitecore/John", false);

      var provider = Substitute.For<Sitecore.Security.Accounts.RolesInRolesProvider>();
      provider.IsUserInRole(user, role, true).Returns(true);

      // switch the roles-in-roles provider so the mocked version is used
      using (new Sitecore.FakeDb.Security.Accounts.RolesInRolesSwitcher(provider))
      {
        Xunit.Assert.True(user.IsInRole(role));
      }
    }

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

    [Fact]
    public void HowToSwitchContextUser()
    {
      using (new Sitecore.Security.Accounts.UserSwitcher(@"extranet\John", true))
      {
        Xunit.Assert.Equal(@"extranet\John", Sitecore.Context.User.Name);
      }
    }

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

    [Fact]
    public void HowToSetUserIsAdministrator()
    {
      var user = Substitute.For<Sitecore.Security.Accounts.User>(@"extranet\John", true);
      user.IsAdministrator.Returns(true);

      Xunit.Assert.True(user.IsAdministrator);
    }

    [Fact]
    public void HowToMockUserProfile()
    {
      var user = Substitute.For<Sitecore.Security.Accounts.User>(@"extranet\John", true);
      user.Profile.Returns(Substitute.For<Sitecore.Security.UserProfile>());

      user.Profile.ClientLanguage.Returns("da");
      user.Profile.Email.Returns("john@mail.com");

      Xunit.Assert.Equal("da", user.Profile.ClientLanguage);
      Xunit.Assert.Equal("john@mail.com", user.Profile.Email);
    }

    #endregion

    #region Pipelines

    [Fact]
    public void HowToUnitTestPipelineCallWithMockedProcessor()
    {
      var args = new Sitecore.Pipelines.PipelineArgs();

      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
      {
        // create pipeline processor mock and register it in the Pipeline Watcher
        var processor = Substitute.For<Sitecore.FakeDb.Pipelines.IPipelineProcessor>();
        db.PipelineWatcher.Register("mypipeline", processor);

        // call the pipeline
        Sitecore.Pipelines.CorePipeline.Run("mypipeline", args);

        // and check the mocked processor received the Process method call with proper arguments
        processor.Received().Process(args);
      }
    }

    [Fact]
    public void HowToUnitTestAdvancedPipelineCallWithMockedProcessor()
    {
      var args = new Sitecore.Pipelines.PipelineArgs();

      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
      {
        // create pipeline processor mock and register it in the Pipeline Watcher
        var processor = Substitute.For<Sitecore.FakeDb.Pipelines.IPipelineProcessor>();
        processor
          .When(p => p.Process(args))
          .Do(ci => ci.Arg<Sitecore.Pipelines.PipelineArgs>().CustomData["Result"] = "Ok");

        db.PipelineWatcher.Register("mypipeline", processor);

        // call the pipeline
        Sitecore.Pipelines.CorePipeline.Run("mypipeline", args);

        // and check the result
        Assert.Equal("Ok", args.CustomData["Result"]);
      }
    }

    /// <summary>
    /// Imagine you have a product repository. The repository should be able to get a 
    /// product by id. The implementation of the repository is 'thin' and does nothing 
    /// else than calling a corresponding pipeline with proper arguments. The next 
    /// example shows how to unit test the pipeline calls (please note that the 
    /// pipeline is not defined in config file):
    /// </summary>
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

    /// <summary>
    /// How to configure the pipeline behaviour.
    /// The code sample above checks that the pipeline is called with proper arguments. 
    /// The next scenario would be to validate the pipeline call results. 
    /// In the code below we configure pipeline proressor behaviour to return an expected
    /// product only if the product id is set to "1".
    /// </summary>
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

    #endregion

    #region Configuration

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

    /// <summary>
    /// By default Sitecore set `singleInstance="true"` for all databases so that each 
    /// of the three default databases behaves as singletones. This approach has list 
    /// of pros and cons; it is important to be avare about potential issues that may 
    /// appear.
    /// 
    /// Single instance allows one to resolve a database in any place of code using 
    /// Sitecore Factory. The same content is available no matter how many times the 
    /// database has been resolved. The next code creates item Home using simplified 
    /// FakeDb API and then reads the item from database resolved from Factory:
    /// </summary>
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

    #endregion

    #region Miscellaneous

    [Fact]
    public void HowToSwitchContextSite()
    {
      // Create a fake Site Context and configure the required parameters.
      // Please note that there is no registration in the App.config file required.
      var fakeSite = new Sitecore.FakeDb.Sites.FakeSiteContext(
        new Sitecore.Collections.StringDictionary
          {
            { "name", "website" }, { "database", "web" }
          });

      // switch the context site
      using (new Sitecore.FakeDb.Sites.FakeSiteContextSwitcher(fakeSite))
      {
        Xunit.Assert.Equal("website", Sitecore.Context.Site.Name);
        Xunit.Assert.Equal("web", Sitecore.Context.Site.Database.Name);
        Xunit.Assert.Equal("website", Factory.GetSite("website").Name);
      }
    }

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

#if SC80 || SC81 || SC811
    [Fact]
    public void HowToSwitchLinkProvider()
    {
      // arrange
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home")
        })
      {
        Sitecore.Data.Items.Item home = db.GetItem("/sitecore/content/home");

        Sitecore.Links.LinkProvider provider = Substitute.For<Sitecore.Links.LinkProvider>();
        provider.GetItemUrl(home, Arg.Is<Sitecore.Links.UrlOptions>(x => x.AlwaysIncludeServerUrl))
          .Returns("http://myawesomeurl.com");

        using (new Sitecore.FakeDb.Links.LinkProviderSwitcher(provider))
        {
          // act
          var result = Sitecore.Links.LinkManager.GetItemUrl(home,
            new Sitecore.Links.UrlOptions { AlwaysIncludeServerUrl = true });

          // assert
          Xunit.Assert.Equal("http://myawesomeurl.com", result);
        }
      }
    }
#endif

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

    [Fact]
    public void HowToWorkWithQueryApi()
    {
      const string Query = "/sitecore/content/*[@@key = 'home']";

      using (new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home")
        })
      {
        Sitecore.Data.Items.Item[] result =
          Sitecore.Data.Query.Query.SelectItems(Query);

        Xunit.Assert.Equal(result.Count(), 1);
        Xunit.Assert.Equal(result[0].Key, "home");
      }
    }

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

    [Fact]
    public void HowToMockContentSearchLogic()
    {
      var index = Substitute.For<Sitecore.ContentSearch.ISearchIndex>();

      // don't forget to clean up.
      Sitecore.ContentSearch
        .ContentSearchManager.SearchConfiguration.Indexes["my_index"] = index;

      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home")
        })
      {
        // configure a search result item behavior.
        var searchResultItem =
          Substitute.For<Sitecore.ContentSearch.SearchTypes.SearchResultItem>();

        var expectedItem = db.GetItem("/sitecore/content/home");
        searchResultItem.GetItem().Returns(expectedItem);

        // configure a search ndex behavior.
        index.CreateSearchContext()
          .GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>()
          .Returns((new[] { searchResultItem }).AsQueryable());

        // get the item from the search index and check the expectations.
        Sitecore.Data.Items.Item actualItem =
          index.CreateSearchContext()
            .GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>()
            .Single()
            .GetItem();

        Xunit.Assert.Equal(expectedItem, actualItem);
      }
    }

    [Fact]
    public void HowToMockIdTable()
    {
      // arrange
      var id = Sitecore.Data.ID.NewID;
      var parentId = Sitecore.Data.ID.NewID;
      var data = "{ }";

      var provider = Substitute.For<Sitecore.Data.IDTables.IDTableProvider>();

      using (new Sitecore.FakeDb.Data.IDTables.IDTableProviderSwitcher(provider))
      {
        // act
        var actualEntry
          = Sitecore.Data.IDTables.IDTable.Add("my_pref", "my_key", id, parentId, data);

        // assert
        Xunit.Assert.Equal("my_pref", actualEntry.Prefix);
        Xunit.Assert.Equal("my_key", actualEntry.Key);
        Xunit.Assert.Equal(id, actualEntry.ID);
        Xunit.Assert.Equal(parentId, actualEntry.ParentID);
        Xunit.Assert.Equal(data, actualEntry.CustomData);
      }
    }

    #endregion

    #region Blobs

    [Fact]
    public void HowToSetAndGetBlobStream()
    {
      // arrange
      var stream = new System.IO.MemoryStream();

      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home")
            {
              new Sitecore.FakeDb.DbField("field")
            }
        })
      {
        Sitecore.Data.Items.Item item = db.GetItem("/sitecore/content/home");
        Sitecore.Data.Fields.Field field = item.Fields["field"];

        using (new Sitecore.Data.Items.EditContext(item))
        {
          // act
          field.SetBlobStream(stream);
        }

        // assert
        Xunit.Assert.Same(stream, field.GetBlobStream());
      }
    }

    #endregion

    #region Translate

    [Fact]
    public void HowToCheckTranslateTextIsCalled()
    {
      const string Phrase = "Welcome!";

      // Enable the 'FakeDb.AutoTranslate' setting.
      // It can be done either statically in the 'App.config' file or 
      // dynamically in a particular test.
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
      {
        db.Configuration.Settings.AutoTranslate = true;

        // translate
        string translatedPhrase = Sitecore.Globalization.Translate.Text(Phrase);

        // note the '*' symbol at the end of the translated phrase
        Xunit.Assert.Equal("Welcome!*", translatedPhrase);
      }
    }

    /// <summary>
    /// FakeDb supports simple localization mechanism. You can call Translate.Text() or
    /// Translate.TextByLanguage() method to get a 'translated' version of the original text.
    /// The translated version has got language name added to the initial phrase.
    /// </summary>
    [Fact]
    public void HowToUnitTestLocalization()
    {
      // init languages
      Sitecore.Globalization.Language en = Sitecore.Globalization.Language.Parse("en");
      Sitecore.Globalization.Language da = Sitecore.Globalization.Language.Parse("da");

      const string Phrase = "Welcome!";

      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
      {
        db.Configuration.Settings.AutoTranslate = true;
        db.Configuration.Settings.AutoTranslatePrefix = "{lang}:";

        // translate
        string enTranslation = Sitecore.Globalization.Translate.TextByLanguage(Phrase, en);
        string daTranslation = Sitecore.Globalization.Translate.TextByLanguage(Phrase, da);

        Xunit.Assert.Equal("en:Welcome!", enTranslation);
        Xunit.Assert.Equal("da:Welcome!", daTranslation);
      }
    }

    #endregion
  }
}