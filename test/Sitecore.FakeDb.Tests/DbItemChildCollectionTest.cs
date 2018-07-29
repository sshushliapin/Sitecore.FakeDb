namespace Sitecore.FakeDb.Tests
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Xunit;

    public class DbItemChildCollectionTest
    {
        [Theory, AutoData]
        public void ShouldAdd(DbItem item, DbItemChildCollection sut)
        {
            sut.Add(item);
            sut.Count.Should().Be(1);
        }

        [Theory, AutoData]
        public void ShouldClear(DbItemChildCollection sut)
        {
            sut.Clear();
            sut.Count.Should().Be(0);
        }

        [Theory, AutoData]
        public void ShouldCheckIfDoesNotContain(DbItem item, DbItemChildCollection sut)
        {
            sut.Contains(item).Should().BeFalse();
        }

        [Theory, AutoData]
        public void ShouldCheckIfContains([Frozen] DbItem item, [Greedy] DbItemChildCollection sut)
        {
            sut.Contains(item).Should().BeTrue();
        }

        [Theory, AutoData]
        public void ShouldCopyTo([Frozen] DbItem item, [Greedy] DbItemChildCollection sut)
        {
            var array = new DbItem[3];

            sut.CopyTo(array, 0);

            array.Should().Contain(item);
        }

        [Theory, AutoData]
        public void ShouldRemove([Frozen] DbItem item, [Greedy] DbItemChildCollection sut)
        {
            sut.Remove(item);
            sut.Count.Should().Be(2);
        }

        [Theory, AutoData]
        public void ShouldCheckIfReadonly(DbItem parent, ReadOnlyCollection<DbItem> items)
        {
            var sut = new DbItemChildCollection(parent, items);
            sut.IsReadOnly.Should().BeTrue();
        }

        [Theory, AutoData]
        public void ShouldCheckIfNotReadonly(DbItemChildCollection sut)
        {
            sut.IsReadOnly.Should().BeFalse();
        }

        [Theory, AutoData]
        public void ShouldGetEnumerator(DbItemChildCollection sut)
        {
            ((IEnumerable) sut).GetEnumerator().Should().NotBeNull();
        }
    }
}