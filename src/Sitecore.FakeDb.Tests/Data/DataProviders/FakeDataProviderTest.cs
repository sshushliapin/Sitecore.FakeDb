namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.AutoNSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.Data.DataProviders;
  using Sitecore.Data.Templates;
  using Sitecore.FakeDb.Data.DataProviders;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.StringExtensions;
  using Xunit;

  public class FakeDataProviderTest : IDisposable
  {
    private readonly FakeDataProvider dataProvider;

    private readonly DataStorageSwitcher dataStorageSwitcher;

    public FakeDataProviderTest()
    {
      var fixture = new Fixture();
      fixture.Customize(
        new CompositeCustomization(
          new DefaultConventions(),
          new AutoNSubstituteCustomization(),
          new AutoConfiguredNSubstituteCustomization()
        ));

      this.dataStorageSwitcher = fixture.Create<DataStorageSwitcher>();
      this.dataProvider = new FakeDataProvider();
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
      this.dataProvider.DataStorage.GetFakeItem(def.ID).Returns(item);

      Action action = () => this.dataProvider.ChangeTemplate(def, changes, null);

      action.ShouldThrow<InvalidOperationException>()
            .WithMessage("Unable to change item template. The target template is not found.");
    }

    [Theory, AutoData]
    public void ShouldGetTemplateIds(DbTemplate template)
    {
      this.dataProvider.DataStorage.GetFakeTemplates().Returns(new[] { template });
      this.dataProvider.GetTemplateItemIds(null).Should().Contain(template.ID);
    }

    [Theory, AutoData]
    public void ShouldGetTemplatesFromDataStorage(DbTemplate template)
    {
      this.dataProvider.DataStorage.GetFakeTemplates().Returns(new[] { template });
      this.dataProvider.GetTemplates(null).Should().HaveCount(1);
    }

    [Theory, AutoData]
    public void ShouldGetTemplatesWithDefaultDataSectionFromDataStorage(DbTemplate template)
    {
      this.dataProvider.DataStorage.GetFakeTemplates().Returns(new[] { template });

      var result = this.dataProvider.GetTemplates(null).First();

      result.GetSection("Data").Should().NotBeNull();
    }

    [Theory, AutoData]
    public void ShouldHaveStandardBaseTemplate([NoAutoProperties]DbTemplate template)
    {
      this.dataProvider.DataStorage.GetFakeTemplates().Returns(new[] { template });

      var result = this.dataProvider.GetTemplates(null).First();

      result.BaseIDs.Single().Should().Be(TemplateIDs.StandardTemplate);
    }

    [Theory, AutoData]
    public void ShouldGetTemplateFields(DbTemplate template)
    {
      this.dataProvider.DataStorage.GetFakeTemplates().Returns(new[] { template });
      template.Fields.Add("Title");

      var result = this.dataProvider.GetTemplates(null).First();

      result.GetField("Title").Should().NotBeNull();
    }

    [Theory, AutoData]
    public void ShouldGetTemplateFieldType(DbTemplate template)
    {
      this.dataProvider.DataStorage.GetFakeTemplates().Returns(new[] { template });
      template.Fields.Add(new DbField("Link") { Type = "General Link" });

      var result = this.dataProvider.GetTemplates(null).First();

      result.GetField("Link").Type.Should().Be("General Link");
    }

    [Theory, AutoData]
    public void ShouldGetTemplateFieldIsShared(DbTemplate template)
    {
      this.dataProvider.DataStorage.GetFakeTemplates().Returns(new[] { template });
      template.Fields.Add(new DbField("Title") { Shared = true });

      var result = this.dataProvider.GetTemplates(null).First();

      result.GetField("Title").IsShared.Should().BeTrue();
    }

    [Theory, DefaultAutoData]
    public void ShouldGetDefaultLanguage(CallContext context)
    {
      var langs = this.dataProvider.GetLanguages(context);

      langs.Should().HaveCount(1);
      langs.First().Name.Should().Be("en");
    }

    [Theory, DefaultAutoData]
    public void ShouldGetItemDefinition(DbItem item, CallContext context)
    {
      // arrange
      this.dataProvider.DataStorage.GetFakeItem(item.ID).Returns(item);

      // act
      var definition = this.dataProvider.GetItemDefinition(item.ID, context);

      // assert
      definition.ID.Should().Be(item.ID);
      definition.Name.Should().Be(item.Name);
      definition.TemplateID.Should().Be(item.TemplateID);
      definition.BranchId.Should().Be(ID.Null);
    }

    [Theory, DefaultAutoData]
    public void ShouldGetNullItemDefinitionIfNoItemFound(ID itemId, CallContext context)
    {
      this.dataProvider.GetItemDefinition(itemId, context).Should().BeNull();
    }

    [Theory, DefaultAutoData]
    public void ShouldGetAllThePossibleItemVersions(ItemDefinition def, CallContext context)
    {
      // arrange
      var item = new DbItem("home", def.ID, def.TemplateID)
                   {
                     Fields =
                       {
                         new DbField("Field 1") { { "en", 1, string.Empty }, { "en", 2, string.Empty }, { "da", 1, string.Empty } },
                         new DbField("Field 2") { { "en", 1, string.Empty }, { "da", 1, string.Empty }, { "da", 2, string.Empty } }
                       }
                   };

      this.dataProvider.DataStorage.GetFakeItem(def.ID).Returns(item);

      // act
      var versions = this.dataProvider.GetItemVersions(def, context);

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

    [Theory, DefaultAutoData]
    public void ShouldGetEmptyVersionsIfNoFakeItemFound(ItemDefinition def, CallContext context)
    {
      this.dataProvider.GetItemVersions(def, context).Should().BeEmpty();
    }

    public void Dispose()
    {
      this.dataStorageSwitcher.Dispose();
    }
  }
}