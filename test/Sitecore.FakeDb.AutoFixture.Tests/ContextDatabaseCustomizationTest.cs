namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using global::AutoFixture;
  using Xunit;

  public class ContextDatabaseCustomizationTest
  {
    [Fact]
    public void CustomizeThrowsIfFixtureIsNull()
    {
      var sut = new ContextDatabaseCustomization();
      Assert.Throws<ArgumentNullException>(() => sut.Customize(null));
    }

    [Fact]
    public void CustomizeAddsContextDatabaseSpecimenBuilder()
    {
      var fixture = new Fixture();
      var sut = new ContextDatabaseCustomization();

      sut.Customize(fixture);

      fixture.Customizations.SingleOrDefault(x => x.GetType() == typeof(ContextDatabaseSpecimenBuilder)).Should().NotBeNull();
    }
  }
}