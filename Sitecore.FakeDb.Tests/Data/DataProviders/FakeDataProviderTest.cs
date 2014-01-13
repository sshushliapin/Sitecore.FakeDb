namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.DataProviders;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Templates;
  using Sitecore.Reflection;
  using Xunit;

  // TODO:[High] To think how to instantiate and configure data provider in the test instead of using a real instance from config file.
  public class FakeDataProviderTest
  {
    private readonly FakeDataProvider dataProvider;

    private readonly DataStorage dataStorage;

    public FakeDataProviderTest()
    {
      this.dataProvider = new FakeDataProvider();

      var database = new FakeDatabase("master");
      this.dataStorage = database.DataStorage;

      ReflectionUtil.CallMethod(database, "AddDataProvider", new object[] { this.dataProvider });
    }

    [Fact]
    public void ShouldGetTemplatesFromDataStorage()
    {
      // arrange
      var t1 = new FTemplate("t1", ID.NewID);
      this.dataStorage.FakeTemplates.Add(t1.ID, t1);

      var t2 = new FTemplate("t2", ID.NewID);
      this.dataStorage.FakeTemplates.Add(t2.ID, t2);

      // act
      var templates = dataProvider.GetTemplates(null);

      // assert
      templates.Should().HaveCount(2);

      templates[0].Name.Should().Be(t1.Name);
      templates[0].ID.Should().Be(t1.ID);

      templates[1].Name.Should().Be(t2.Name);
      templates[1].ID.Should().Be(t2.ID);
    }

    [Fact]
    public void ShouldGetTemplatesWithDefaultDataSectionFromDataStorage()
    {
      // arrange
      var t = new FTemplate();
      this.dataStorage.FakeTemplates.Add(t.ID, t);

      var template = this.dataProvider.GetTemplates(null).First();

      // act & assert
      template.GetSection("Data").Should().NotBeNull();
    }

    [Fact]
    public void ShouldGetTemplateFields()
    {
      // arrange
      var t = new FTemplate { "Title", "Description" };
      this.dataStorage.FakeTemplates.Add(t.ID, t);

      var template = this.dataProvider.GetTemplates(null).First();

      // act & assert
      template.GetField("Title").Should().NotBeNull();
    }
  }
}