namespace Sitecore.FakeDb.Tests
{
  using System;
  using Xunit;

  public class WellknownFieldsTest
  {
    [Fact]
    public void ShouldBeReadonlyDictionary()
    {
      // act & assert
      Assert.Throws<NotSupportedException>(() => WellknownFields.FieldIdToNameMapping.Clear());
    }
  }
}