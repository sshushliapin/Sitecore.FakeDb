namespace Sitecore.FakeDb.Tests.Data.Engines
{
  using System.Collections.Generic;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Globalization;
  using Xunit;
  using Version = Sitecore.Data.Version;

  public class SwitchingDataStorageTest
  {
    [Theory, DefaultAutoData]
    public void IsDataStorage(SwitchingDataStorage sut)
    {
      sut.Should().BeAssignableTo<DataStorage>();
    }

    [Theory, DefaultAutoData]
    public void GetFakeItemCallsCurrentStorage(SwitchingDataStorage sut, DataStorage currentStorage, ID itemId, DbItem expected)
    {
      using (new DataStorageSwitcher(currentStorage))
      {
        currentStorage.GetFakeItem(itemId).Returns(expected);

        sut.GetFakeItem(itemId).Should().BeSameAs(expected);
      }
    }

    [Theory, DefaultAutoData]
    public void AddFakeItemCallsCurrentStorage(SwitchingDataStorage sut, DataStorage currentStorage, DbItem item)
    {
      using (new DataStorageSwitcher(currentStorage))
      {
        sut.AddFakeItem(item);

        currentStorage.Received().AddFakeItem(item);
      }
    }

    [Theory, DefaultAutoData]
    public void GetFakeTemplateCallsCurrentStorage(SwitchingDataStorage sut, DataStorage currentStorage, ID templateId, DbTemplate expected)
    {
      using (new DataStorageSwitcher(currentStorage))
      {
        currentStorage.GetFakeTemplate(templateId).Returns(expected);

        sut.GetFakeTemplate(templateId).Should().BeSameAs(expected);
      }
    }

    [Theory, DefaultAutoData]
    public void GetFakeTemplatesCallsCurrentStorage(SwitchingDataStorage sut, DataStorage currentStorage, List<DbTemplate> expected)
    {
      using (new DataStorageSwitcher(currentStorage))
      {
        currentStorage.GetFakeTemplates().Returns(expected);

        sut.GetFakeTemplates().Should().BeSameAs(expected);
      }
    }

    [Theory, DefaultAutoData]
    public void GetSitecoreItemCallsCurrentStorage(SwitchingDataStorage sut, DataStorage currentStorage, ID itemId, Language language, Version version, [NoAutoProperties] Item expected)
    {
      using (new DataStorageSwitcher(currentStorage))
      {
        currentStorage.GetSitecoreItem(itemId, language, version).Returns(expected);

        sut.GetSitecoreItem(itemId, language, version).Should().BeSameAs(expected);
      }
    }
  }
}