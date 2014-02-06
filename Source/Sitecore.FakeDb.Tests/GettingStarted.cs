namespace Examples
{
  using FluentAssertions;
  using Sitecore.FakeDb;
  using Xunit;

  public class GettingStarted
  {
    [Fact]
    public void HowDoICreateASimpleItem()
    {
      using (var db = new Db { new DbItem("Home") { { "Title", "Welcome!" } } })
      {
        Sitecore.Data.Items.Item homeItem = db.GetItem("/sitecore/content/home");
        homeItem["Title"].Should().Be("Welcome!");
      }
    }

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

    [Fact]
    public void ShouldSetAsContextItem()
    {
      using (var db = new Db { new DbItem("Home") })
      {
        var item = db.GetItem("/sitecore/content/home");
        using (new Sitecore.Data.Items.ContextItemSwitcher(item))
        {
          Sitecore.Context.Item.Should().Be(item);
        }

        Sitecore.Context.Item.Should().BeNull();
      }
    }
  }
}