namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class DbFieldCollectionTest
  {
    [Fact]
    public void ShouldAddDbField()
    {
      // arrange
      var collection = new DbFieldCollection { new DbField("Title") };

      // act & assert
      collection.Count().Should().Be(1);
    }

    [Fact]
    public void ShouldGetFieldById()
    {
      // arrange
      var id = ID.NewID;
      var field = new DbField("Title", id);
      var collection = new DbFieldCollection { field };

      // act & assert
      collection[id].ShouldBeEquivalentTo(field);
    }

    [Fact]
    public void ShouldSetFieldById()
    {
      // arrange
      var id = ID.NewID;
      var originalField = new DbField("Title", id);
      var newField = new DbField("Title", id);

      var collection = new DbFieldCollection { originalField };

      // act
      collection[id] = newField;

      // assert
      collection[id].ShouldBeEquivalentTo(newField);
    }

    [Fact]
    public void ShouldAddFieldByName()
    {
      // arrange & act
      var collection = new DbFieldCollection { "field1", "field2" };

      // assert
      collection.ElementAt(0).Name.Should().Be("field1");
      collection.ElementAt(0).ID.Should().NotBeNull();
      collection.ElementAt(0).Value.Should().BeEmpty();

      collection.ElementAt(1).Name.Should().Be("field2");
      collection.ElementAt(1).ID.Should().NotBeNull();
      collection.ElementAt(1).Value.Should().BeEmpty();
    }

    [Fact]
    public void ShouldAddFieldByNameAndValue()
    {
      // arrange & act
      var collection = new DbFieldCollection { { "field1", "value1" }, { "field2", "value2" } };

      // assert
      collection.ElementAt(0).Name.Should().Be("field1");
      collection.ElementAt(0).ID.Should().NotBeNull();
      collection.ElementAt(0).Value.Should().Be("value1");

      collection.ElementAt(1).Name.Should().Be("field2");
      collection.ElementAt(1).ID.Should().NotBeNull();
      collection.ElementAt(1).Value.Should().Be("value2");
    }

    [Fact]
    public void ShouldThrowExceptionIfNoFieldIdPresent()
    {
      // arrange
      var collection = new DbFieldCollection();
      var missingFieldId = ID.NewID;
      var expectedMessage = string.Format("The given field \"{0}\" is not present in the item.", missingFieldId);

      // act & assert
      Assert.Throws<InvalidOperationException>(() => collection[missingFieldId])
        .Message.Should().Be(expectedMessage);
    }

    [Fact]
    public void ShouldReturnTrueIfContainsField()
    {
      // arrange
      var fieldId = ID.NewID;
      var collection = new DbFieldCollection { new DbField(fieldId) };

      // act & assert
      collection.ContainsKey(fieldId).Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalseIfNoField()
    {
      // arrange
      var collection = new DbFieldCollection();

      // act & assert
      collection.ContainsKey(ID.NewID).Should().BeFalse();
    }

    [Fact]
    public void ShouldGetValues()
    {
      // arrange
      var field1 = new DbField("field1");
      var field2 = new DbField("field2");
      var collection = new DbFieldCollection { field1, field2 };

      // act & assert
      collection.ShouldAllBeEquivalentTo(new[] { field1, field2 });
    }
  }
}