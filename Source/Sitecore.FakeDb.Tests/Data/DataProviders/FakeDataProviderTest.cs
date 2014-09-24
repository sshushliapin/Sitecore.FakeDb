namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.DataProviders;
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
      var database = Database.GetDatabase("master");
      this.dataStorage = new DataStorage(database);
      this.dataStorage.FakeItems.Clear();
      this.dataStorage.FakeTemplates.Clear();

      this.dataProvider = new FakeDataProvider(this.dataStorage);
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
      this.CreateTestTemplateInDataStorage();

      var result = this.dataProvider.GetTemplates(null).First();

      // act & assert
      result.GetSection("Data").Should().NotBeNull();
    }

    [Fact]
    public void ShouldGetTemplateWithBaseTemplateField()
    {
      // arrange
      var template = this.CreateTestTemplateInDataStorage();

      // act
      var result = this.dataProvider.GetTemplates(null).First();

      // assert
      result.GetField("__Base template").Should().NotBeNull();
      result.GetField(FieldIDs.BaseTemplate).Should().NotBeNull();
    }

    [Fact]
    public void ShouldGetTemplateFields()
    {
      // arrange
      var template = this.CreateTestTemplateInDataStorage();
      template.Fields.Add("Title");

      var result = this.dataProvider.GetTemplates(null).First();

      // act & assert
      result.GetField("Title").Should().NotBeNull();
    }

    [Fact]
    public void ShouldGetTemplateFieldType()
    {
      // arrange
      var template = this.CreateTestTemplateInDataStorage();
      template.Fields.Add(new DbField("Link") { Type = "General Link" });

      var result = this.dataProvider.GetTemplates(null).First();

      // act & assert
      result.GetField("Link").Type.Should().Be("General Link");
      result.GetField("Link").TypeKey.Should().Be("general link");
    }

    [Fact]
    public void ShouldGetTemplateFieldIsShared()
    {
      // arrange
      var template = this.CreateTestTemplateInDataStorage();
      template.Fields.Add(new DbField("Title") { Shared = true });

      var result = this.dataProvider.GetTemplates(null).First();

      // act & assert
      result.GetField("Title").IsShared.Should().BeTrue();
    }

    [Fact]
    public void ShouldBeIRequireDataStorage()
    {
      // assert
      this.dataProvider.Should().BeAssignableTo<IRequireDataStorage>();
    }

    [Fact]
    public void ShouldSetDataStorage()
    {
      // arrange
      var ds = new DataStorage(Database.GetDatabase("master"));

      // act
      this.dataProvider.SetDataStorage(ds);

      // assert
      this.dataProvider.DataStorage.Should().Be(ds);
    }

    [Fact]
    public void ShouldGetDefaultLanguage()
    {
      // arrange
      var db = this.dataStorage.Database;

      // act
      var langs = this.dataProvider.GetLanguages(new CallContext(db.DataManager, db.GetDataProviders().Count()));

      // assert
      langs.Should().HaveCount(1);
      langs.First().Name.Should().Be("en");
    }

    [Fact]
    public void ShouldGetItemDefinition()
    {
      // arrange
      var itemId = ID.NewID;
      var templateId = ID.NewID;
      var callContext = new CallContext(new DataManager(this.dataStorage.Database), 1);

      this.dataStorage.FakeItems.Add(itemId, new DbItem("home", itemId, templateId));

      // act
      var definition = this.dataProvider.GetItemDefinition(itemId, callContext);

      // assert
      definition.ID.Should().Be(itemId);
      definition.Name.Should().Be("home");
      definition.TemplateID.Should().Be(templateId);
      definition.BranchId.Should().Be(ID.Null);
    }

    [Fact]
    public void ShouldGetNullItemDefinitionIfNoItemFound()
    {
      // arrange
      var itemId = ID.NewID;
      var callContext = new CallContext(new DataManager(this.dataStorage.Database), 1);

      // act & assert
      this.dataProvider.GetItemDefinition(itemId, callContext).Should().BeNull();
    }

    [Fact]
    public void ShouldGetAllThePossibleItemVersions()
    {
      // arrange
      var itemId = ID.NewID;
      var templateId = ID.NewID;

      var definition = new ItemDefinition(itemId, "home", templateId, ID.Null);
      var callContext = new CallContext(new DataManager(this.dataStorage.Database), 1);

      var item = new DbItem("home", itemId, templateId)
                   {
                     Fields =
                       {
                         new DbField("Field 1") { { "en", 1, string.Empty }, { "en", 2, string.Empty }, { "da", 1, string.Empty } },
                         new DbField("Field 2") { { "en", 1, string.Empty }, { "da", 1, string.Empty }, { "da", 2, string.Empty } }
                       }
                   };

      this.dataStorage.FakeItems.Add(itemId, item);

      // act
      var versions = this.dataProvider.GetItemVersions(definition, callContext);

      // assert
      versions.Count.Should().Be(4);
      versions[0].Language.Name.Should().Be("en");
      versions[0].Version.Number.Should().Be(1);
      versions[1].Language.Name.Should().Be("en");
      versions[1].Version.Number.Should().Be(2);
      versions[2].Language.Name.Should().Be("da");
      versions[2].Version.Number.Should().Be(1);
      versions[3].Language.Name.Should().Be("da");
      versions[3].Version.Number.Should().Be(2);
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