namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class DbLinkFieldTest
  {
    [Fact]
    public void ShouldBeDbField()
    {
      // arrange
      var field = new DbLinkField("Link");

      // assert
      field.Should().BeAssignableTo<DbField>();
    }

    [Fact]
    public void ShouldCreateEmptyDbLink()
    {
      // arrange
      var linkField = new DbLinkField("Link");

      // assert
      linkField.Anchor.Should().BeEmpty();
      linkField.Class.Should().BeEmpty();
      linkField.LinkType.Should().BeEmpty();
      linkField.QueryString.Should().BeEmpty();
      linkField.Target.Should().BeEmpty();
      linkField.TargetID.Should().Be(ID.Null);
      linkField.Text.Should().BeEmpty();
      linkField.Title.Should().BeEmpty();
      linkField.Url.Should().BeEmpty();
    }

    [Fact]
    public void ShouldSetAndGetLinkFieldAttributes()
    {
      // arrange
      var targetId = ID.NewID;

      // act
      var linkField = new DbLinkField("Link")
                        {
                          Anchor = "anchor",
                          Class = "class",
                          LinkType = "linktype",
                          QueryString = "querystring",
                          Target = "target",
                          TargetID = targetId,
                          Text = "text",
                          Title = "title",
                          Url = "url",
                        };

      // assert
      linkField.Anchor.Should().Be("anchor");
      linkField.Class.Should().Be("class");
      linkField.LinkType.Should().Be("linktype");
      linkField.QueryString.Should().Be("querystring");
      linkField.Target.Should().Be("target");
      linkField.TargetID.Should().Be(targetId);
      linkField.Text.Should().Be("text");
      linkField.Title.Should().Be("title");
      linkField.Url.Should().Be("url");
    }

    [Fact]
    public void ShouldStoreInternalLinkAttributesIntoValue()
    {
      // arrange
      var targetId = new ID("{AA011160-CE64-4F24-A389-22CE5C3A5935}");

      // act
      var field = new DbLinkField("Link")
                    {
                      Anchor = "anchor",
                      Class = "class",
                      LinkType = "linktype",
                      QueryString = "querystring",
                      Target = "target",
                      TargetID = targetId,
                      Text = "text",
                      Title = "title",
                      Url = "url",
                    };

      // assert
      field.Value.Should().Be("<link anchor=\"anchor\" class=\"class\" linktype=\"linktype\" querystring=\"querystring\" target=\"target\" id=\"{AA011160-CE64-4F24-A389-22CE5C3A5935}\" text=\"text\" title=\"title\" url=\"url\" />");
    }
  }
}