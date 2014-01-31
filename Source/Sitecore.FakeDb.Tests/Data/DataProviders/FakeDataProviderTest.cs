namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.DataProviders;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Reflection;
  using Xunit;

  public class FakeDataProviderTest
  {
    private readonly FakeDataProvider dataProvider;

    private readonly DataStorage dataStorage;

    public FakeDataProviderTest()
    {
      var database = new FakeDatabase("master");

      this.dataStorage = Substitute.For<DataStorage>();
      database.DataStorage = this.dataStorage;

      this.dataProvider = new FakeDataProvider();
      ReflectionUtil.CallMethod(database, "AddDataProvider", new object[] { this.dataProvider });
    }

    [Fact]
    public void ShouldGetTemplateIds()
    {
      // arrange
      var t1 = this.CreateTestTemplateInDataStorage();
      var t2 = this.CreateTestTemplateInDataStorage();

      // act
      var templateIds = this.dataProvider.GetTemplateItemIds(null);

      // assert
      templateIds.Count.Should().Be(2);
      templateIds.Should().Contain(t1.ID);
      templateIds.Should().Contain(t2.ID);
    }

    [Fact]
    public void ShouldGetTemplatesFromDataStorage()
    {
      // arrange
      var t1 = this.CreateTestTemplateInDataStorage();
      var t2 = this.CreateTestTemplateInDataStorage();

      // act
      var templates = this.dataProvider.GetTemplates(null);

      // assert
      templates.Count.Should().Be(2);
      templates.GetTemplate(t1.ID).Name.Should().Be(t1.Name);
      templates.GetTemplate(t2.ID).Name.Should().Be(t2.Name);
    }

    [Fact]
    public void ShouldGetTemplatesWithDefaultDataSectionFromDataStorage()
    {
      // arrange
      var t = new DbTemplate();
      this.dataStorage.FakeTemplates.Add(t.ID, t);

      var template = this.dataProvider.GetTemplates(null).First();

      // act & assert
      template.GetSection("Data").Should().NotBeNull();
    }

    [Fact]
    public void ShouldGetTemplateFields()
    {
      // arrange
      var t = new DbTemplate { "Title", "Description" };
      this.dataStorage.FakeTemplates.Add(t.ID, t);

      var template = this.dataProvider.GetTemplates(null).First();

      // act & assert
      template.GetField("Title").Should().NotBeNull();
    }

    private DbTemplate CreateTestTemplateInDataStorage()
    {
      var templateId = ID.NewID;
      var template = new DbTemplate(templateId.ToString(), templateId);
      this.dataStorage.FakeTemplates.Add(template.ID, template);

      return template;
    }
  }
}