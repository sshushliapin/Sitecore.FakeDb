namespace Sitecore.FakeDb.Tests
{
  using Xunit;

  public class GettingStarted
  {
    [Fact]
    public void CreateNewItem()
    {
      using (Db db = new Db
                       {
                         new DbItem("home") { { "Title", "Welcome!" } }
                       })
      {
        Sitecore.Data.Items.Item homeItem = db.GetItem("/sitecore/content/home");

        Assert.NotNull(homeItem);
        Assert.Equal("Welcome!", homeItem["Title"]);
      }
    }

    [Fact]
    public void CreateAndEditSimpleItem()
    {
      using (Db db = new Db { new DbItem("home") { "Title" } })
      {
        var item = db.GetItem("/sitecore/content/home");
        using (new Sitecore.Data.Items.EditContext(item))
        {
          item["Title"] = "Welcome!";
        }

        Assert.Equal("Welcome!", item["Title"]);
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
        Assert.NotNull(gettingStartedArticle);
        Assert.Equal("Articles helping to get started.", gettingStartedArticle["Description"]);

        Sitecore.Data.Items.Item troubleshootingArticle = articles.Children["Troubleshooting"];
        Assert.NotNull(troubleshootingArticle);
        Assert.Equal("Articles with solutions to common problems.", troubleshootingArticle["Description"]);
      }
    }
  }
}