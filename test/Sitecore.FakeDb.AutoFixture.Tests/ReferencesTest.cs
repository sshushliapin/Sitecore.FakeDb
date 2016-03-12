namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using Xunit;

  public class ReferencesTest
  {
    /// <summary>
    /// https://github.com/sergeyshushlyapin/Sitecore.FakeDb/issues/96
    /// </summary>
    [Fact]
    public void ShouldReferenceSpecificAutoFixtureVersion()
    {
      typeof(AutoDbCustomization).Assembly.GetReferencedAssemblies()
        .First(x => x.Name == "Ploeh.AutoFixture")
        .Version.Should().Be(new Version(3, 30, 0, 0));
    }
  }
}