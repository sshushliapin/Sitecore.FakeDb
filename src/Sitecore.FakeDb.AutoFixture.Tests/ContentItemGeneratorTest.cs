namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class ContentItemGeneratorTest
  {
    [Fact]
    public void ShouldBeSpecimenBuilder()
    {
      // arrange
      using (var db = new Db())
      {
        // act
        var sut = new ContentItemGenerator(db);

        // assert
        sut.Should().BeAssignableTo<ISpecimenBuilder>();
      }
    }

    [Fact]
    public void ShouldThrowIfDbIsNull()
    {
      // act
      Action action = () => new ContentItemGenerator(null);

      // assert
      action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void ShouldReturnNoSpecimenIdNotItem()
    {
      // arrange
      using (var db = new Db())
      {
        var sut = new ContentItemGenerator(db);

        // act
        var result = sut.Create(new object(), null);

        // assert
        result.Should().BeOfType<NoSpecimen>();
      }
    }
  }

  public class ContentItemGeneratorCommandTest
  {
    [Fact]
    public void ShouldAddItemToDatabase()
    {
      // arrange
      using (var db = new Db())
      {
        var sut = new ContentItemGeneratorCommand(db);
        var item = ItemHelper.CreateInstance();

        // act
        sut.Execute(item, null);

        // assert
        db.GetItem("/sitecore/content").Children.Should().HaveCount(1);
      }
    }


    [Fact]
    public void ShouldAddDbItemToDatabase()
    {
      // arrange
      using (var db = new Db())
      {
        var sut = new ContentItemGeneratorCommand(db);
        var item = new DbItem("home");

        // act
        sut.Execute(item, null);

        // assert
        db.GetItem("/sitecore/content").Children.Should().HaveCount(1);
      }
    }
  }
}