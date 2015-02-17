namespace Examples.Serialization
{
  using Xunit;

  public class GettingStarted
  {
    [Fact]
    public void HowToDeserializeItem()
    {
      using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
        {
          new Sitecore.FakeDb.Serialization.DsDbTemplate(
            "/sitecore/templates/Sample/Sample Item"),
          new Sitecore.FakeDb.Serialization.DsDbItem(
            "/sitecore/content/home", true)
        })
      {
        var home = db.GetItem("/sitecore/content/home");
        Assert.Equal("Sitecore", home["Title"]);
      }
    }
  }
}