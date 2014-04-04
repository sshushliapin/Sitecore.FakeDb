namespace Sitecore.FakeDb.Tests.Data
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Configuration;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Xunit;

  public class FakeStandardValuesProviderTest : IDisposable
  {
    private readonly FakeStandardValuesProvider provider;

    public FakeStandardValuesProviderTest()
    {
      provider = new FakeStandardValuesProvider();
    }

    [Fact]
    public void ShouldRequireDataStorage()
    {
      // act & assert
      provider.Should().BeAssignableTo<IRequireDataStorage>();
    }

    [Fact]
    public void ShouldSetDataStorage()
    {
      // arrange
      var storage = Substitute.For<DataStorage>(Factory.GetDatabase("master"));

      // act
      ((IRequireDataStorage)provider).SetDataStorage(storage);

      // assert
      ((IRequireDataStorage)provider).DataStorage.Should().BeSameAs(storage);
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}