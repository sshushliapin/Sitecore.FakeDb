namespace Sitecore.FakeDb.Tests.Data.IDTables
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Sitecore.Data;
  using Sitecore.Data.IDTables;
  using Sitecore.FakeDb.Data.IDTables;
  using Xunit;

  public class FakeIDTableProviderTest : IDisposable
  {
    private readonly FakeIDTableProvider provider;

    private readonly IDTableProvider behavior;

    private readonly IDTableEntry entry;

    public FakeIDTableProviderTest()
    {
      this.behavior = Substitute.For<IDTableProvider>();
      this.provider = new FakeIDTableProvider();
      this.provider.LocalProvider.Value = this.behavior;

      var fixture = new Fixture();
      this.entry = fixture.Create<IDTableEntry>();
    }

    [Fact]
    public void ShouldBeThreadLocalProvider()
    {
      // act & assert
      this.provider.Should().BeAssignableTo<IThreadLocalProvider<IDTableProvider>>();
    }

    [Fact]
    public void ShouldReturnEmptyValuesWithoutBehaviorSet()
    {
      // arrange
      var stubProvider = new FakeIDTableProvider();

      // act & assert
      stubProvider.Add(null);
      stubProvider.GetID(null, null).Should().BeNull();
      stubProvider.GetKeys(null, null).Should().BeEmpty();
      stubProvider.Remove(null, null);
    }

    [Fact]
    public void ShouldCallAdd()
    {
      // act
      this.provider.Add(this.entry);

      // assert
      this.behavior.Received().Add(this.entry);
    }

    [Fact]
    public void ShouldCallGetId()
    {
      // arrange
      this.behavior.GetID("prefix", "key").Returns(this.entry);

      // act && assert
      this.provider.GetID("prefix", "key").Should().BeSameAs(this.entry);
    }

    [Fact]
    public void ShouldCallGetKeys()
    {
      // arrange
      var keys = new[] { this.entry };
      var id = ID.NewID;

      // arrange
      this.behavior.GetKeys("prefix", id).Returns(keys);

      // act && assert
      this.provider.GetKeys("prefix", id).Should().BeSameAs(keys);
    }

    [Fact]
    public void ShouldCallRemove()
    {
      // act
      this.provider.Remove("prefix", "key");

      // assert
      this.behavior.Received().Remove("prefix", "key");
    }

    public void Dispose()
    {
      this.provider.Dispose();
    }
  }
}