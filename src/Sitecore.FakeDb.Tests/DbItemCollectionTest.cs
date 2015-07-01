namespace Sitecore.FakeDb.Tests
{
  using System.Collections.ObjectModel;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class DbItemCollectionTest
  {
    [Theory, AutoData]
    public void ShouldAdd(DbItem item, DbItemCollection sut)
    {
      // act
      sut.Add(item);

      // assert
      sut.Count.Should().Be(1);
    }

    [Theory, AutoData]
    public void ShouldClear(DbItemCollection sut)
    {
      // act
      sut.Clear();

      // assert
      sut.Count.Should().Be(0);
    }

    [Theory, AutoData]
    public void ShouldCheckIfDoesNotContain(DbItem item, DbItemCollection sut)
    {
      // act
      var result = sut.Contains(item);

      // assert
      result.Should().BeFalse();
    }

    [Theory, AutoData]
    public void ShouldCheckIfContains([Frozen]DbItem item, [Greedy]DbItemCollection sut)
    {
      // act
      var result = sut.Contains(item);

      // assert
      result.Should().BeTrue();
    }

    [Theory, AutoData]
    public void ShouldCopyTo([Frozen]DbItem item, [Greedy]DbItemCollection sut)
    {
      // arrange
      var array = new DbItem[3];

      // act
      sut.CopyTo(array, 0);

      // assert
      array.Should().Contain(item);
    }

    [Theory, AutoData]
    public void ShouldRemove([Frozen] DbItem item, [Greedy]DbItemCollection sut)
    {
      // act
      sut.Remove(item);

      // assert
      sut.Count.Should().Be(2);
    }

    [Theory, AutoData]
    public void ShouldCheckIfReadonly(ReadOnlyCollection<DbItem> items)
    {
      // arrange
      var sut = new DbItemCollection(items);

      // act
      var result = sut.IsReadOnly;

      // assert
      result.Should().BeTrue();
    }

    [Theory, AutoData]
    public void ShouldCheckIfNotReadonly(DbItemCollection sut)
    {
      // act
      var result = sut.IsReadOnly;

      // assert
      result.Should().BeFalse();
    }
  }
}