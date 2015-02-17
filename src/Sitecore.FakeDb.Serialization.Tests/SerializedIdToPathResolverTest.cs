namespace Sitecore.FakeDb.Serialization.Tests
{
  using System.IO;
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class SerializedIdToPathResolverTest
  {
    [Fact]
    public void ShouldFindPathForId()
    {
      string filePath = ID.Parse("{108266C2-304B-4AD3-9813-2BE1B88609FF}").FindFilePath("custom");

      filePath.Should().NotBeNullOrWhiteSpace();
      File.Exists(filePath).Should().BeTrue();
      Path.GetFileNameWithoutExtension(filePath).ShouldBeEquivalentTo("Item only available in custom serialization folder");
    }
  }
}