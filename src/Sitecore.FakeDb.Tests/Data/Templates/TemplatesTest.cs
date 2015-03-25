namespace Sitecore.FakeDb.Tests.Data.Templates
{
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class TemplatesTest
  {
    [Fact]
    public void ShouldGetOwnFields()
    {
      // arrange
      var templateId = ID.NewID;

      using (var db = new Db
                        {
                          new DbTemplate(templateId) { "expected own field" },
                          new DbItem("home", ID.NewID, templateId)
                        })
      {
        var item = db.GetItem("/sitecore/content/home");

        // assert
        item.Template.OwnFields.Count().Should().Be(1, string.Join("\n", item.Template.OwnFields.Select(f => f.Name)));
        item.Template.OwnFields.Single().Name.Should().Be("expected own field");
      }
    }
  }
}