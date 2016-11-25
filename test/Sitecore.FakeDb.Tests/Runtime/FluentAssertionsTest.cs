namespace Sitecore.FakeDb.Tests.Runtime
{
  using System;
  using FluentAssertions;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class FluentAssertionsTest
  {
    [Fact]
    public void ShouldNotTimeOutOnFailingFluentAssertion()
    {
      // arrange
      using (var db = new Db
      {
        new DbItem("home")
      })
      {
        var home = db.GetItem("/sitecore/content/home");

        Action assertion = () =>
        {
          try { home.Should().BeNull(); } catch { }
        };

        // act && assert
        assertion.ExecutionTime().ShouldNotExceed(1.Seconds());
      }
    }
  }
}