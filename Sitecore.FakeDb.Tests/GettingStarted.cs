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
    public void CreateItemHieararchy()
    {
      using (Db db = new Db
                        {
                          new DbItem("home")
                            {
                              new DbItem("articles")
                                {
                                  new DbItem("latest news")
                                }
                            }
                        })
      {
        var item = db.GetItem("/sitecore/content/home/articles/latest news");

        Assert.NotNull(item);
      }
    }
  }
}