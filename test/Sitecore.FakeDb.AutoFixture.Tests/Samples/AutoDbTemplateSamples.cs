namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class AutoDbTemplateSamples
  {
    [Theory, AutoDbData]
    public void CreateItemBasedOnCustomTemplate(
      [Content] Item root,
      [Content] MyHomeTemplate template)
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

    public class MyHomeTemplate : DbTemplate
    {
      public MyHomeTemplate()
      {
        this.Add("Title");
      }
    }
  }
}