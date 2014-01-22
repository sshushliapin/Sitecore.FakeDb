namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Data.Items;
  using Xunit;

  public class GettingStarted
  {
    [Fact]
    public void CreateAndEditSimpleItem()
    {
      using (var db = new Db { new DbItem("home") { "Title" } })
      {
        var item = db.GetItem("/sitecore/content/home");

        using (new EditContext(item))
        {
          item["Title"] = "Welcome!";
        }

        item["Title"].Should().Be("Welcome!");
      }
    }

    [Fact]
    public void CreateItemHieararchy()
    {
      using (var db = new Db
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

        item.Should().NotBeNull();
      }
    }
  }
}