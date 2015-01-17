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
      var field = new DbLinkField("Extrnal Url");

      // assert
      field.Should().BeAssignableTo<DbField>();
    }

    /// <summary>
    /// Example: <link text="Link to Home item" linktype="internal" class="default" title="Home" target='Active Browser' querystring="sc_lang=en" id="{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}" />
    /// </summary>
    [Fact]
    public void ShouldSetInternalLinkAttributes()
    {
      // arrange & act
      var field = new DbLinkField("Internal Link")
                    {
                      Text = "Link to Home item",
                      LinkType = "internal",
                      Class = "default",
                      Title = "Home",
                      Target = "Active Browser",
                      QueryString = "sc_lang=en",
                      TargetID = new ID("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
                    };

      // assert
      field.Value.Should().Be("<link text=\"Link to Home item\" linktype=\"internal\" class=\"default\" title=\"Home\" target=\"Active Browser\" querystring=\"sc_lang=en\" id=\"{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}\" />");
    }

    /// <summary>
    /// Example: <link text="Gmail" linktype="external" url="http://gmail.com" anchor="" title="Google mail" class="link" target="Active Browser" />
    /// </summary>
    [Fact]
    public void ShouldSetExternalLinkAttributes()
    {
      // arrange & act
      var field = new DbLinkField("External Link")
                    {
                      Text = "Gmail",
                      LinkType = "external",
                      Url = "http://gmail.com",
                      Anchor = string.Empty,
                      Title = "Google mail",
                      Class = "link",
                      Target = "Active Browser"
                    };

      // assert
      field.Value.Should().Be("<link text=\"Gmail\" linktype=\"external\" url=\"http://gmail.com\" anchor=\"\" title=\"Google mail\" class=\"link\" target=\"Active Browser\" />");
    }
  }
}