namespace Examples
{
  using System.Linq;
  using NSubstitute;

  public class GettingStarted
  {
    #region Content

    [Xunit.Fact]
    public void HowToCreateSimpleItem()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("Home") { { "Title", "Welcome!" } }
        })
      {
        Sitecore.Data.Items.Item homeItem = db.GetItem("/sitecore/content/home");
        Xunit.Assert.Equal("Welcome!", homeItem["Title"]);
      }
    }

    [Xunit.Fact]
    public void HowToCreateItemUnderSystem()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home") { ParentID = Sitecore.ItemIDs.SystemRoot }
        })
      {
        Sitecore.Data.Items.Item home = db.GetItem("/sitecore/system/home");
        Xunit.Assert.Equal("home", home.Key);
      }
    }

    [Xunit.Fact]
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

        Xunit.Assert.NotNull(articles["Getting Started"]);
        Xunit.Assert.NotNull(articles["Troubleshooting"]);
      }
    }

    [Xunit.Fact]
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

    [Xunit.Fact]
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

    [Xunit.Fact]
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

    [Xunit.Fact]
    public void HowToCreateTemplateWithStandardValues()
    {
      var templateId = new Sitecore.Data.TemplateID(Sitecore.Data.ID.NewID);

      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbTemplate("sample", templateId) { { "Title", "$name" } }
        })
      {
        Sitecore.Data.Items.Item contentRoot = db.GetItem(Sitecore.ItemIDs.ContentRoot);
        Sitecore.Data.Items.Item item = contentRoot.Add("Home", templateId);

        Xunit.Assert.Equal("Home", item["Title"]);
      }
    }

    [Xunit.Fact]
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
        var template = Sitecore.Data.Managers.TemplateManager.GetTemplate(templateId, db.Database);

        Xunit.Assert.Contains(baseTemplateIdOne, template.BaseIDs);
        Xunit.Assert.Contains(baseTemplateIdTwo, template.BaseIDs);

        Xunit.Assert.True(template.InheritsFrom(baseTemplateIdOne));
        Xunit.Assert.True(template.InheritsFrom(baseTemplateIdTwo));
      }
    }

    #endregion

    #region Links

    [Xunit.Fact]
    public void HowToWorkWithLinkDatabase()
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
        Xunit.Assert.Equal(Sitecore.Globals.LinkDatabase.GetReferrers(source).Count(), 0);

        using (new Sitecore.FakeDb.Links.LinkDatabaseSwitcher(behavior))
        {
          Sitecore.Links.ItemLink[] referrers = Sitecore.Globals.LinkDatabase.GetReferrers(source);

          Xunit.Assert.Equal(referrers.Count(), 2);
          Xunit.Assert.Equal(referrers.Count(r => r.SourceItemID == clone.ID && r.TargetItemID == source.ID), 1);
          Xunit.Assert.Equal(referrers.Count(r => r.SourceItemID == alias.ID && r.TargetItemID == source.ID), 1);
        }

        // link database is clean again
        Xunit.Assert.Equal(Sitecore.Globals.LinkDatabase.GetReferrers(source).Count(), 0);
      }
    }

    #endregion

    #region Security

    [Xunit.Fact]
    public void HowToMockAuthenticationProvider()
    {
      // create and configure authentication provider mock
      var provider = Substitute.For<Sitecore.Security.Authentication.AuthenticationProvider>();
      provider.Login("John", true).Returns(true);

      // switch the authentication provider so the mocked version is used
      using (new Sitecore.Security.Authentication.AuthenticationSwitcher(provider))
      {
        // the authentication manager is called with the expected parameters. It returns 'true'
        Xunit.Assert.True(Sitecore.Security.Authentication.AuthenticationManager.Login("John", true));

        // the authentication manager is called with some unexpected parameters. It returns 'false'
        Xunit.Assert.False(Sitecore.Security.Authentication.AuthenticationManager.Login("Robber", true));
      }
    }

    [Xunit.Fact]
    public void HowToMockRoleProvider()
    {
      // create and configure role provider mock
      string[] roles = { @"sitecore/Authors", @"sitecore/Editors" };

      System.Web.Security.RoleProvider provider = Substitute.For<System.Web.Security.RoleProvider>();
      provider.GetAllRoles().Returns(roles);

      // switch the role provider so the mocked version is used
      using (new Sitecore.FakeDb.Security.Web.RoleProviderSwitcher(provider))
      {
        string[] resultRoles = System.Web.Security.Roles.GetAllRoles();

        Xunit.Assert.True(resultRoles.Contains(@"sitecore/Authors"));
        Xunit.Assert.True(resultRoles.Contains(@"sitecore/Editors"));
      }
    }

    [Xunit.Fact]
    public void HowToSwitchContextUser()
    {
      using (new Sitecore.Security.Accounts.UserSwitcher(@"extranet\John", true))
      {
        Xunit.Assert.Equal(@"extranet\John", Sitecore.Context.User.Name);
      }
    }

    [Xunit.Fact]
    public void HowToConfigureItemAccess()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("home") { Access = { CanRead = false } }
        })
      {
        Sitecore.Data.Items.Item item = db.GetItem("/sitecore/content/home");

        // item is null because read is denied
        Xunit.Assert.Null(item);
      }
    }

    #endregion

    #region Pipelines

    /// <summary>
    /// How to ensure the pipeline is called with specific argsuments.
    /// Imagine you have a product repository. The repository should be able to get a product
    /// by id. The implementation of the repository is 'thin' and does nothing than calling a
    /// corresponding pipeline with proper arguments. The next example shows how to unit test
    /// the pipeline call (please note that the pipeline is not defined in the tests assembly
    /// config file):
    /// </summary>
    [Xunit.Fact]
    public void HowToEnsurePipelineIsCalledWithSpecificArgs()
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
    [Xunit.Fact]
    public void HowToConfigurePipelineBehaviour()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db())
      {
        // create a product to get from the repository
        object expectedProduct = new object();

        // configure processing of the pipeline arguments. Will set the 'expectedProduct'
        // instance to CustomData["Product"] property only when the CustomData["ProductId"]
        // is "1"
        string productId = "1";

        // configure a pipeline watcher to expect a pipeline call where the args custom data
        // contains ProductId. Once the args received the pipeline result is set into
        // Product custom data property
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

    #region Globalization

    /// <summary>
    /// FakeDb supports simple localization mechanism. You can call Translate.Text() or
    /// Translate.TextByLanguage() method to get a 'translated' version of the original text.
    /// The translated version has got language name added to the initial phrase.
    /// </summary>
    [Xunit.Fact]
    public void HowToTranslateTexts()
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

    #endregion

    #region Configuration

    [Xunit.Fact]
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

    [Xunit.Fact]
    public void HowToGetItemFromSitecoreDatabase()
    {
      using (new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.DbItem("Home")
        })
      {
        Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("master");
        Xunit.Assert.NotNull(database.GetItem("/sitecore/content/home"));
      }
    }

    #endregion

    #region Media

    [Xunit.Fact]
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

    #endregion

    #region Miscellaneous

    [Xunit.Fact]
    public void HowToWorkWithQueryApi()
    {
      using (var db = new Sitecore.FakeDb.Db { new Sitecore.FakeDb.DbItem("home") })
      {
        var query = "/sitecore/content/*[@@key = 'home']";

        Sitecore.Data.Items.Item[] result;
        using (new Sitecore.Data.DatabaseSwitcher(db.Database))
        {
          result = Sitecore.Data.Query.Query.SelectItems(query);
        }

        Xunit.Assert.Equal(result.Count(), 1);
        Xunit.Assert.Equal(result[0].Key, "home");
      }
    }

    [Xunit.Fact]
    public void HowToMockContentSearchLogic()
    {
      try
      {
        var index = Substitute.For<Sitecore.ContentSearch.ISearchIndex>();
        Sitecore.ContentSearch.ContentSearchManager.SearchConfiguration.Indexes.Add("my_index", index);

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
        Sitecore.ContentSearch.ContentSearchManager.SearchConfiguration.Indexes.Remove("my_index");
      }
    }

    #endregion
  }
}