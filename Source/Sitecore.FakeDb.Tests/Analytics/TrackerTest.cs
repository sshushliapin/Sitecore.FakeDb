namespace Sitecore.FakeDb.Tests.Analytics
{
  using System;
  using FluentAssertions;
  using Sitecore.Analytics;
  using Sitecore.Analytics.Data.DataAccess;
  using Sitecore.Common;
  using Xunit;

  public class TrackerTest
  {
    [Fact]
    public void ShouldGetCurrentVisitor()
    {
      // arrange
      var visitorId = Guid.NewGuid();

      // act
      using (new Switcher<Visitor>(new Visitor(visitorId)))
      {
        // assert
        Tracker.Visitor.VisitorId.Should().Be(visitorId);
      }
    }
  }
}