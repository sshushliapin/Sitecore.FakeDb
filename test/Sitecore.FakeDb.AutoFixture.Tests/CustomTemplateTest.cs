namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using global::AutoFixture;
  using global::AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class CustomTemplateTest
  {
    [Theory, AutoDbData]
    public void CreateItemBasedOnCustomTemplate([Content] Item root, [Content] MyHomeTemplate template)
    {
      // act
      var home = root.Add("home", new TemplateID(template.ID));
      using (new EditContext(home))
      {
        home["Title"] = "Welcome AutoFixture!";
      }

      // assert
      Assert.Equal("Welcome AutoFixture!", home["Title"]);
    }

    private class AutoDbDataAttribute : AutoDataAttribute
    {
      public AutoDbDataAttribute()
        : base(() => new Fixture().Customize(new AutoDbCustomization()))
      {
      }
    }

    public class MyHomeTemplate : DbTemplate
    {
      public MyHomeTemplate()
      {
        this.Add("Title");
      }
    }
  }
}