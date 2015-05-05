namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Xunit;

  /// <summary>
  /// Internal link: <link text="Link to Home item" linktype="internal" class="default" title="Home" target='Active Browser' querystring="sc_lang=en" id="{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}" />
  /// External link: <link text="Gmail" linktype="external" url="http://gmail.com" anchor="" title="Google mail" class="link" target="Active Browser" />
  /// </summary>
  public class LinkFieldTest
  {
    [Fact]
    public void ShouldSetLinkFieldPropertiesUsingRawValue()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home")
                            {
                              { "link", "<link linktype=\"external\" url=\"http://google.com\" />" }
                            }
                        })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        var linkField = (LinkField)item.Fields["link"];

        // assert
        linkField.LinkType.Should().Be("external");
        linkField.Url.Should().Be("http://google.com");
      }
    }

    [Fact]
    public void ShouldSetLinkFieldPropertiesUsingDbLinkField()
    {
      // arrange
      var targetId = ID.NewID;

      using (var db = new Db
                        {
                          new DbItem("home")
                            {
                              new DbLinkField("link")
                                {
                                  LinkType = "internal", 
                                  QueryString = "sc_lang=en",
                                  TargetID = targetId,
                                }
                            }
                        })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        var linkField = (LinkField)item.Fields["link"];

        // assert
        linkField.Should().NotBeNull("'item.Fields[\"link\"]' should not be null");
        linkField.LinkType.Should().Be("internal");
        linkField.QueryString.Should().Be("sc_lang=en");
        linkField.TargetID.Should().Be(targetId);
      }
    }

    [Fact]
    public void ShouldGetTargetItem()
    {
      // arrange
      var targetId = ID.NewID;

      using (var db = new Db
                        {
                          new DbItem("home")
                            {
                              new DbLinkField("link") { TargetID = targetId }
                            },
                          new DbItem("target", targetId)
                        })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        var linkField = (LinkField)item.Fields["link"];

        // assert
        linkField.Should().NotBeNull("'item.Fields[\"link\"]' should not be null");
        linkField.TargetItem.Name.Should().Be("target");
      }
    }

    [Theory]
    [InlineData("internal", true)]
    [InlineData("media", false)]
    public void ShouldCheckIfInternal(string linkType, bool isinternal)
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home")
                            {
                              new DbLinkField("link") { LinkType = linkType }
                            }
                        })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        var linkField = (LinkField)item.Fields["link"];

        // assert
        linkField.Should().NotBeNull("'item.Fields[\"link\"]' should not be null");
        linkField.IsInternal.Should().Be(isinternal);
      }
    }
  }
}