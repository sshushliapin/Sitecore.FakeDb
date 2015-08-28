namespace Sitecore.FakeDb.Tests.Data
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class FakeStandardValuesProviderTest
  {
    [Fact]
    public void ShouldRequireDataStorage()
    {
      // arrange
      var sut = new FakeStandardValuesProvider();

      // act & assert
      sut.Should().BeAssignableTo<IRequireDataStorage>();
    }

    [Fact]
    public void ShouldReturnEmptyStringIfNoTemplateFound()
    {
      // arrange
      var storage = Substitute.For<DataStorage>(Database.GetDatabase("master"));
      var field = new Field(ID.NewID, ItemHelper.CreateInstance());
      var sut = new FakeStandardValuesProvider();

      using (new DataStorageSwitcher(storage))
      {
        // act & assert
        sut.GetStandardValue(field).Should().BeEmpty();
      }
    }

    [Fact]
    public void ShouldThrowIfNoDataStorageSet()
    {
      // arrange
      var field = new Field(ID.NewID, ItemHelper.CreateInstance());
      var sut = new FakeStandardValuesProvider();

      // act
      Action action = () => sut.GetStandardValue(field);

      // assert
      action.ShouldThrow<InvalidOperationException>()
            .WithMessage("DataStorage cannot be null.");
    }
  }
}