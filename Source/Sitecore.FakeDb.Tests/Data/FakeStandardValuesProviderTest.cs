namespace Sitecore.FakeDb.Tests.Data
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class FakeStandardValuesProviderTest : IDisposable
  {
    private readonly FakeStandardValuesProvider provider;

    private readonly Database database;

    public FakeStandardValuesProviderTest()
    {
      this.provider = new FakeStandardValuesProvider();
      this.database = Factory.GetDatabase("master");
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
      var storage = Substitute.For<DataStorage>(this.database);

      // act
      ((IRequireDataStorage)this.provider).SetDataStorage(storage);

      // assert
      ((IRequireDataStorage)this.provider).DataStorage.Should().BeSameAs(storage);
    }

    [Fact]
    public void ShouldReturnEmptyStringIfNoTemplateFound()
    {
      // arrange
      var storage = Substitute.For<DataStorage>(this.database);
      ((IRequireDataStorage)this.provider).SetDataStorage(storage);

      var field = new Field(ID.NewID, ItemHelper.CreateInstance(this.database));

      // act & assert
      this.provider.GetStandardValue(field).Should().BeEmpty();
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}