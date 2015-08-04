namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Xunit;

  public class AutoTemplateTest
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
      home["Title"].Should().Be("Welcome AutoFixture!");
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