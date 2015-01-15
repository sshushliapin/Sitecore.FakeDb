namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Xunit;

  public class LinkFieldTest
  {
    [Fact]
    public void ShouldGetEmptyInternalLinkFieldByDefault()
    {
      // arrange
      const string fieldName = "External Url";

      using (var db = new Db
                        {
                          new DbItem("home") { { fieldName, "" } }
                        })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        var linkField = (LinkField)item.Fields[fieldName];

        // assert
        linkField.Anchor.Should().BeEmpty("Anchor");
        linkField.Class.Should().BeEmpty("Class");
        linkField.InnerField.Name.Should().Be(fieldName);
        linkField.InternalPath.Should().BeEmpty("InternalPath");
        linkField.IsInternal.Should().BeTrue("IsInternal");
        linkField.IsMediaLink.Should().BeFalse("IsMediaLink");
        linkField.LinkType.Should().Be("internal");
        linkField.MediaPath.Should().BeEmpty("MediaPath");
        linkField.QueryString.Should().BeEmpty("QueryString");
        linkField.Root.Should().Be("link");
        linkField.Target.Should().BeEmpty("Target");
        linkField.TargetID.Should().Be(ID.Null);
        linkField.TargetItem.Should().BeNull("TargetItem");
        linkField.Text.Should().BeEmpty("Text");
        linkField.Title.Should().BeEmpty("Title");
        linkField.Url.Should().BeEmpty("Url");
        linkField.Value.Should().BeEmpty("Value");
        linkField.Xml.Should().BeNull("Xml");
      }
    }
  }
}
