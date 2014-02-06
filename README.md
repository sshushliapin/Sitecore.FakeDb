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
    
### What can I do with Sitecore FakeDb

Sitecore FakeDb allows you to perform the next manipulations with items in memory:

* create and edit items
* read items by path or id
* access item children
* access item parent
* delete items
