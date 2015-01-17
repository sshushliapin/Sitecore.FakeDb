namespace Sitecore.FakeDb
{
  using FluentAssertions;
  using Sitecore.FakeDb.Tests;
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

    [Fact]
    public void ShouldSetBasicFields()
    {
      // arrange
      var field = new DbLinkField("External Url")
                    {
                      Url = "http://mycorp.com"
                    };

      // assert
      field.Value.Should().Be("<link url=\"http://mycorp.com\" />");
    }
  }
}