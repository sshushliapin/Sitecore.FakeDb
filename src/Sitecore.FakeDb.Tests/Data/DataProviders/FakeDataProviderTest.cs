namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.DataProviders;
  using Sitecore.Data.Templates;
  using Sitecore.FakeDb.Data.DataProviders;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Reflection;
  using Sitecore.StringExtensions;
  using Xunit;

  public class FakeDataProviderTest : IDisposable
  {
    private readonly FakeDataProvider dataProvider;

    private readonly DataStorage dataStorage;

    private readonly DataStorageSwitcher dataStorageSwitcher;

    public FakeDataProviderTest()
    {
      var database = Database.GetDatabase("master");
      this.dataStorage = Substitute.For<DataStorage>(database);
      this.dataStorageSwitcher = new DataStorageSwitcher(this.dataStorage);
      this.dataProvider = new FakeDataProvider();
      ReflectionUtil.CallMethod(database, "AddDataProvider", new object[] { this.dataProvider });
    }

    [Theory, DefaultAutoData]
    public void ChangeTemplateThrowsIfItemDefinitionIsNull()
    {
      Action action = () => this.dataProvider.ChangeTemplate(null, null, null);
      action.ShouldThrow<ArgumentNullException>();
    }

    [Theory, DefaultAutoData]
    public void ChangeTemplateThrowsIfTemplateChangeListIsNull(ItemDefinition def)
    {
      Action action = () => this.dataProvider.ChangeTemplate(def, null, null);
      action.ShouldThrow<ArgumentNullException>();
    }

    [Theory, DefaultAutoData]
    public void ChangeTemplateThrowsIfNoDbItemFound(ItemDefinition def, TemplateChangeList changes)
    {
      Action action = () => this.dataProvider.ChangeTemplate(def, changes, null);
      action.ShouldThrow<InvalidOperationException>()
            .WithMessage("Unable to change item template. The item '{0}' is not found.".FormatWith(def.ID));
    }

    [Theory, DefaultAutoData]
    public void ChangeTemplateThrowsIfNoTargetTemplateFound(ItemDefinition def, TemplateChangeList changes, DbItem item)
    {
      this.dataStorage.GetFakeItem(def.ID).Returns(item);

      Action action = () => this.dataProvider.ChangeTemplate(def, changes, null);

      action.ShouldThrow<InvalidOperationException>()
            .WithMessage("Unable to change item template. The target template is not found.");
    }

    [Fact]
    public void ShouldGetTemplateIds()
    {
      // arrange
      var template = this.CreateTestTemplateInDataStorage();

      // act
      var templateIds = this.dataProvider.GetTemplateItemIds(null);

      // assert
      templateIds.Should().Contain(template.ID);
    }

    [Fact]
    public void ShouldGetTemplatesFromDataStorage()
    {
      // arrange
      var template = this.CreateTestTemplateInDataStorage();

      // act
      var templates = this.dataProvider.GetTemplates(null);

      // assert
      templates.GetTemplate(template.ID).Name.Should().Be(template.Name);
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
    public void ShouldHaveStandardBaseTemplate()
    {
      // arrange
      this.CreateTestTemplateInDataStorage();

      // act
      var result = this.dataProvider.GetTemplates(null).First();

      // assert
      result.BaseIDs.Single().Should().Be(TemplateIDs.StandardTemplate);
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
      var callContext = this.GetCallContext();

      this.dataStorage.GetFakeItem(itemId).Returns(new DbItem("home", itemId, templateId));

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
      var callContext = this.GetCallContext();

      var item = new DbItem("home", itemId, templateId)
                   {
                     Fields =
                       {
                         new DbField("Field 1") { { "en", 1, string.Empty }, { "en", 2, string.Empty }, { "da", 1, string.Empty } },
                         new DbField("Field 2") { { "en", 1, string.Empty }, { "da", 1, string.Empty }, { "da", 2, string.Empty } }
                       }
                   };

      this.dataStorage.GetFakeItem(itemId).Returns(item);

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

    [Fact]
    public void ShouldGetEmptyVersionsIfNoFakeItemFound()
    {
      // arrange
      var itemId = ID.NewID;
      var templateId = ID.NewID;

      var definition = new ItemDefinition(itemId, "home", templateId, ID.Null);
      var callContext = this.GetCallContext();

      // act & assert
      new FakeDataProvider().GetItemVersions(definition, callContext).Should().BeEmpty();
    }

    public void Dispose()
    {
      this.dataStorageSwitcher.Dispose();
    }

    private DbTemplate CreateTestTemplateInDataStorage()
    {
      var templateId = ID.NewID;
      var template = new DbTemplate(templateId.ToString(), templateId);
      this.dataStorage.GetFakeTemplates().Returns(new[] { template });

      return template;
    }

    private CallContext GetCallContext()
    {
      return new CallContext(new DataManager(this.dataStorage.Database), 1);
    }
  }
}