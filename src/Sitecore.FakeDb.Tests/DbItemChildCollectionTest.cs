namespace Sitecore.FakeDb.Tests
{
  using System.Collections.ObjectModel;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class DbItemChildCollectionTest
  {
    [Theory, AutoData(typeof(OmitOnRecursionFixture))]
    public void ShouldAdd(DbItem item, DbItemChildCollection sut)
    {
      // act
      sut.Add(item);

      // assert
      sut.Count.Should().Be(1);
    }

    [Theory, AutoData(typeof(OmitOnRecursionFixture))]
    public void ShouldClear(DbItemChildCollection sut)
    {
      // act
      sut.Clear();

      // assert
      sut.Count.Should().Be(0);
    }

    [Theory, AutoData(typeof(OmitOnRecursionFixture))]
    public void ShouldCheckIfDoesNotContain(DbItem item, DbItemChildCollection sut)
    {
      // act
      var result = sut.Contains(item);

      // assert
      result.Should().BeFalse();
    }

    [Theory, AutoData(typeof(OmitOnRecursionFixture))]
    public void ShouldCheckIfContains([Frozen]DbItem item, [Greedy]DbItemChildCollection sut)
    {
      // act
      var result = sut.Contains(item);

      // assert
      result.Should().BeTrue();
    }

    [Theory, AutoData(typeof(OmitOnRecursionFixture))]
    public void ShouldCopyTo([Frozen]DbItem item, [Greedy]DbItemChildCollection sut)
    {
      // arrange
      var array = new DbItem[3];

      // act
      sut.CopyTo(array, 0);

      // assert
      array.Should().Contain(item);
    }

    [Theory, AutoData(typeof(OmitOnRecursionFixture))]
    public void ShouldRemove([Frozen] DbItem item, [Greedy]DbItemChildCollection sut)
    {
      // act
      sut.Remove(item);

      // assert
      sut.Count.Should().Be(2);
    }

    [Theory, AutoData(typeof(OmitOnRecursionFixture))]
    public void ShouldCheckIfReadonly([Frozen] DbItem parent, ReadOnlyCollection<DbItem> items)
    {
      // arrange
      var sut = new DbItemChildCollection(parent, items);

      // act
      var result = sut.IsReadOnly;

      // assert
      result.Should().BeTrue();
    }

    [Theory, AutoData(typeof(OmitOnRecursionFixture))]
    public void ShouldCheckIfNotReadonly(DbItemChildCollection sut)
    {
      // act
      var result = sut.IsReadOnly;

      // assert
      result.Should().BeFalse();
    }
  }
}