namespace Sitecore.FakeDb.Tests
{
  using Xunit;

  public class GettingStarted
  {
    [Fact]
    public void CreateAndEditSimpleItem()
    {
      // Let's create a fake in-memory database. The code below creates new template 'Home' with default section 'Data' and single field 'Title'.
      // Then it creates item 'Home' based on the template and sets the 'Title' field value to 'Welcome!':
      using (Db db = new Db { new DbItem("home") { { "Title", "Welcome!" } } })
      {
        // do smth important here.

        // Now we can access Sitecore database inside of the 'using' statement. By default 'master' database is used:
        Sitecore.Data.Database database = db.Database;

        // The database can also be resolved in native Sitecore style:
        database = Sitecore.Data.Database.GetDatabase("master");

        // Now we can access the 'Home' item by path. By default all the items created under '/sitecore/content':
        Sitecore.Data.Items.Item homeItem = database.GetItem("/sitecore/content/home");

        // It is possible to get the value of the 'Title' field:
        string title = homeItem["Title"];
        Assert.Equal("Welcome!", title);

        // The value can also be accessed using item fields collection:
        title = homeItem.Fields["Title"].Value;
        Assert.Equal("Welcome!", title);

        // Now we can update the field with some new value:
        using (new Sitecore.Data.Items.EditContext(homeItem))
        {
          homeItem["Title"] = "Hi there!";
        }

        Assert.Equal("Hi there!", homeItem["Title"]);

        // Having an item it is easy to add a new child. The code below creates new item 'About' under the home item and sets title:
        var templateId = new Sitecore.Data.TemplateID(homeItem.TemplateID);
        var aboutItem = homeItem.Add("About", templateId);

        using (new Sitecore.Data.Items.EditContext(aboutItem))
        {
          aboutItem["Title"] = "About us";
        }

        // When the item is no more needed it can be removed:
        aboutItem.Delete();
        Assert.Null(database.GetItem(aboutItem.ID));
      }
    }

    [Fact]
    public void CreateItemHierarchy()
    {
      using (Db db = new Db
                       {
                         new DbItem("home")
                           {
                             new DbItem("Articles")
                               {
                                 new DbItem("Getting Started") { { "Description", "Articles helping to get started." } },
                                 new DbItem("Troubleshooting") { { "Description", "Articles with solutions to common problems." } }
                               }
                           }
                       })
      {
        Sitecore.Data.Items.Item articles = db.Database.GetItem("/sitecore/content/home/Articles");

        Sitecore.Data.Items.Item gettingStartedArticle = articles.Children["Getting Started"];
        Assert.Equal("Articles helping to get started.", gettingStartedArticle["Description"]);

        Sitecore.Data.Items.Item troubleshootingArticle = articles.Children["Troubleshooting"];
        Assert.Equal("Articles with solutions to common problems.", troubleshootingArticle["Description"]);
      }
    }
  }
}