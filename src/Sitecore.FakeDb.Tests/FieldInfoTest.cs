namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class FieldInfoTest
  {
    [Fact]
    public void EmptyFieldInfoNameIsNull()
    {
      FieldInfo.Empty.Name.Should().BeNull();
    }

    [Fact]
    public void EmptyFieldInfoIdIsEmptyGuid()
    {
      FieldInfo.Empty.Id.Should().BeEmpty();
    }

    [Fact]
    public void EmptyFieldInfoSharedIdFalse()
    {
      FieldInfo.Empty.Shared.Should().BeFalse();
    }

    [Fact]
    public void EmptyFieldInfoTypeIsNull()
    {
      FieldInfo.Empty.Type.Should().BeNull();
    }

    [Theory, AutoData]
    public void ShouldBeEqualIfAllParametersAreEqual(string name, Guid id, bool shared, string type)
    {
      var field1 = new FieldInfo(name, id, shared, type);
      var field2 = new FieldInfo(name, id, shared, type);

      Assert.True(field1 == field2);
      Assert.False(field1 != field2);
      Assert.True(field1.Equals(field2));
    }

    [Theory, AutoData]
    public void ShouldNotBeEqualIfNameDiffers(string name, Guid id, bool shared, string type, string smth)
    {
      var field1 = new FieldInfo(name, id, shared, type);
      var field2 = new FieldInfo(smth, id, shared, type);

      Assert.False(field1 == field2);
      Assert.False(field1.Equals(field2));
    }

    [Theory, AutoData]
    public void ShouldNotBeEqualIfIdDiffers(string name, Guid id, bool shared, string type, Guid smth)
    {
      var field1 = new FieldInfo(name, id, shared, type);
      var field2 = new FieldInfo(name, smth, shared, type);

      Assert.False(field1 == field2);
      Assert.False(field1.Equals(field2));
    }

    [Theory, AutoData]
    public void ShouldNotBeEqualIfSharedDiffers(string name, Guid id, bool shared, string type, bool smth)
    {
      var field1 = new FieldInfo(name, id, shared, type);
      var field2 = new FieldInfo(name, id, smth, type);

      Assert.False(field1 == field2);
      Assert.False(field1.Equals(field2));
    }

    [Theory, AutoData]
    public void ShouldNotBeEqualIfTypeDiffers(string name, Guid id, bool shared, string type, string smth)
    {
      var field1 = new FieldInfo(name, id, shared, type);
      var field2 = new FieldInfo(name, id, shared, smth);

      Assert.False(field1 == field2);
      Assert.False(field1.Equals(field2));
    }

    [Fact]
    public void ShouldNotBeEqualToNull()
    {
      new FieldInfo().Equals(null).Should().BeFalse();
    }

    [Fact]
    public void ShouldGetZeroHashCodeForEmptyFieldInfo()
    {
      new FieldInfo().GetHashCode().Should().Be(0);
    }

    [Theory, AutoData]
    public void ShouldGetHashCode(string name, Guid id, bool shared, string type)
    {
      new FieldInfo(name, id, shared, type).GetHashCode().Should().NotBe(0);
    }
  }
}