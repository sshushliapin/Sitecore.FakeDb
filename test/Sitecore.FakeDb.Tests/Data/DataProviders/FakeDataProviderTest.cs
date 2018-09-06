namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using FluentAssertions;
    using NSubstitute;
    using global::AutoFixture.Xunit2;
    using Sitecore.Collections;
    using Sitecore.Data;
    using Sitecore.Data.DataProviders;
    using Sitecore.Data.Items;
    using Sitecore.Data.Templates;
    using Sitecore.FakeDb.Data.DataProviders;
    using Sitecore.Globalization;
    using Sitecore.StringExtensions;
    using Sitecore.Xml;
    using Xunit;
    using Version = Sitecore.Data.Version;

    public class FakeDataProviderTest
    {
        [Theory, DefaultAutoData]
        public void AddVersionThrowsIfItemDefinitionIsNull(FakeDataProvider sut, VersionUri baseVersion)
        {
            Action action = () => sut.AddVersion(null, baseVersion, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*itemDefinition");
        }

        [Theory, DefaultAutoData]
        public void AddVersionThrowsIfBaseVersionIsNull(FakeDataProvider sut, ItemDefinition itemDefinition)
        {
            Action action = () => sut.AddVersion(itemDefinition, null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*baseVersion");
        }

        [Theory, DefaultAutoData]
        public void AddVersionThrowsIfItemDefinitionNotFound(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            VersionUri baseVersion)
        {
            Action action = () => sut.AddVersion(itemDefinition, baseVersion, null);
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Unable to add item version. The item '{0}' is not found.".FormatWith(itemDefinition.ID));
        }

        [Theory, DefaultAutoData]
        public void AddVersionAddsNewVersionAndReturnsNewVersionNumber(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            Language language,
            int version,
            DbItem item)
        {
            sut.DataStorage.GetFakeItem(itemDefinition.ID).Returns(item);
            var baseVersion = new VersionUri(language, Version.Parse(version));
            var expectedVersion = version + 1;

            var result = sut.AddVersion(itemDefinition, baseVersion, null);

            result.Should().Be(expectedVersion);
            item.GetVersionCount(language.Name).Should().Be(expectedVersion);
        }

        [Theory, DefaultAutoData]
        public void CopyItemThrowsIfSourceIsNull(FakeDataProvider sut)
        {
            Action action = () => sut.CopyItem(null, null, null, ID.Null, null);
            action.ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("source");
        }

        [Theory, DefaultAutoData]
        public void CopyItemThrowsIfDestinationIsNull(
            FakeDataProvider sut,
            ItemDefinition source)
        {
            Action action = () => sut.CopyItem(source, null, null, ID.Null, null);
            action.ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("destination");
        }

        [Theory, DefaultAutoData]
        public void CopyItemThrowsIfCopyNameIsNull(
            FakeDataProvider sut,
            ItemDefinition source,
            ItemDefinition destination)
        {
            Action action = () => sut.CopyItem(source, destination, null, ID.Null, null);
            action.ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("copyName");
        }

        [Theory, DefaultAutoData]
        public void CopyItemThrowsIfCopyIdIsNull(
            FakeDataProvider sut,
            ItemDefinition source,
            ItemDefinition destination,
            string copyName)
        {
            Action action = () => sut.CopyItem(source, destination, copyName, null, null);
            action.ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("copyId");
        }

        [Theory, DefaultAutoData]
        public void CopyItemThrowsIfNoDestinationItemFound(
            [Greedy] FakeDataProvider sut,
            ItemDefinition source,
            ItemDefinition destination,
            string copyName,
            ID copyId,
            CallContext context)
        {
            Action action = () => sut.CopyItem(source, destination, copyName, copyId, context);
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Unable to copy item '{0}'. The source item '{1}' is not found."
                    .FormatWith(copyName, source.ID));
        }

        [Theory, DefaultAutoData]
        public void CopyItemReturnsTrue(
            [Greedy] FakeDataProvider sut,
            ItemDefinition source,
            ItemDefinition destination,
            string copyName,
            ID copyId,
            CallContext context,
            DbItem sourceItem)
        {
            sut.DataStorage.GetFakeItem(source.ID).Returns(sourceItem);
            sut.CopyItem(source, destination, copyName, copyId, context)
                .Should().BeTrue();
        }

        [Theory, DefaultAutoData]
        public void CopyItemAddsCopiedItemToDataStorage(
            [Greedy] FakeDataProvider sut,
            ItemDefinition source,
            ItemDefinition destination,
            string copyName,
            ID copyId,
            CallContext context,
            DbItem sourceItem)
        {
            sut.DataStorage.GetFakeItem(source.ID).Returns(sourceItem);
            sut.CopyItem(source, destination, copyName, copyId, context);
            sut.DataStorage.Received().AddFakeItem(
                Arg.Is<DbItem>(i => i.Name == copyName &&
                                    i.ID == copyId &&
                                    i.TemplateID == source.TemplateID &&
                                    i.ParentID == destination.ID));
        }

        [Theory, DefaultAutoData]
        public void CreateItemThrowsIfItemIdIsNull(FakeDataProvider sut)
        {
            Action action = () => sut.CreateItem(null, null, null, null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*itemId");
        }

        [Theory, DefaultAutoData]
        public void CreateItemThrowsIfItemNameIsNull(FakeDataProvider sut, ID itemId)
        {
            Action action = () => sut.CreateItem(itemId, null, null, null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*itemName");
        }

        [Theory, DefaultAutoData]
        public void CreateItemThrowsIfTemplateIdIsNull(FakeDataProvider sut, ID itemId, string itemName)
        {
            Action action = () => sut.CreateItem(itemId, itemName, null, null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*templateId");
        }

        [Theory, DefaultAutoData]
        public void CreateItemThrowsIfParentIsNull(FakeDataProvider sut, ID itemId, string itemName, ID templateId)
        {
            Action action = () => sut.CreateItem(itemId, itemName, templateId, null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*parent");
        }

        [Theory, DefaultAutoData]
        public void CreateItemAddsFakeItemToDataStorage(
            [Greedy] FakeDataProvider sut,
            ID itemId,
            string itemName,
            ID templateId,
            ItemDefinition parent,
            CallContext context)
        {
            sut.CreateItem(itemId, itemName, templateId, parent, context);
            sut.DataStorage.Received().AddFakeItem(
                Arg.Is<DbItem>(i => i.Name == itemName &&
                                    i.ID == itemId &&
                                    i.TemplateID == templateId &&
                                    i.ParentID == parent.ID));
        }

        [Theory, DefaultAutoData]
        public void CreateItemAddsReturnsTrue(
            [Greedy] FakeDataProvider sut,
            ID itemId,
            string itemName,
            ID templateId,
            ItemDefinition parent,
            CallContext context)
        {
            sut.CreateItem(itemId, itemName, templateId, parent, context).Should().BeTrue();
        }

        [Theory, DefaultAutoData]
        public void ChangeTemplateThrowsIfItemDefinitionIsNull(FakeDataProvider sut)
        {
            Action action = () => sut.ChangeTemplate(null, null, null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Theory, DefaultAutoData]
        public void ChangeTemplateThrowsIfTemplateChangeListIsNull(FakeDataProvider sut, ItemDefinition def)
        {
            Action action = () => sut.ChangeTemplate(def, null, null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Theory, DefaultAutoData]
        public void ChangeTemplateThrowsIfNoDbItemFound(
            [Greedy] FakeDataProvider sut,
            ItemDefinition def)
        {
            var doc = new XmlDocument();
            var template = doc.AppendChild(doc.CreateElement("template"));
            XmlUtil.AddAttribute("name", "template", template);
            var changes = new TemplateChangeList(
                Template.Parse(template, new TemplateCollection()),
                Template.Parse(template, new TemplateCollection()));

            Action action = () => sut.ChangeTemplate(def, changes, null);
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Unable to change item template. The item '{0}' is not found.".FormatWith(def.ID));
        }

        [Theory, DefaultAutoData]
        public void DeleteItemThrowsIfItemDefinitionIsNull(FakeDataProvider sut)
        {
            Action action = () => sut.DeleteItem(null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*itemDefinition");
        }

        [Theory, DefaultAutoData]
        public void DeleteItemReturnsFalseIfNoDbItemFound([Greedy] FakeDataProvider sut, ItemDefinition itemDefinition)
        {
            sut.DeleteItem(itemDefinition, null).Should().BeFalse();
        }

        [Theory, DefaultAutoData]
        public void DeleteItemReturnsTrueIfRemoved(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            DbItem item)
        {
            sut.DataStorage.GetFakeItem(itemDefinition.ID).Returns(item);
            sut.DataStorage.RemoveFakeItem(item.ID).Returns(true);

            sut.DeleteItem(itemDefinition, null).Should().BeTrue();

            sut.DataStorage.Received().RemoveFakeItem(item.ID);
        }

        [Theory, DefaultAutoData]
        public void GetBlobStreamReturnsBlobStreamFromDataStorage(
            [Greedy] FakeDataProvider sut,
            Guid blobId,
            [Modest] MemoryStream stream,
            CallContext context)
        {
            sut.DataStorage.GetBlobStream(blobId).Returns(stream);
            sut.GetBlobStream(blobId, context).Should().BeSameAs(stream);
        }

        [Theory, DefaultAutoData]
        public void GetBlobStreamReturnsNullIfNoBlobStreamExists(
            [Greedy] FakeDataProvider sut,
            Guid blobId,
            CallContext context)
        {
            sut.GetBlobStream(blobId, context).Should().BeNull();
        }

        [Theory, DefaultAutoData]
        public void GetChildIdsThrowsIfItemDefinitionIsNull(
            [Greedy] FakeDataProvider sut,
            CallContext context)
        {
            Action action = () => sut.GetChildIDs(null, context);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*itemDefinition");
        }

        [Theory, DefaultAutoData]
        public void GetChildIdsReturnsEmptyListIfNoItemFound(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            CallContext context)
        {
            sut.GetChildIDs(itemDefinition, context).Should().BeEmpty();
        }

        [Theory, DefaultAutoData]
        public void GetChildIdsReturnsChildIds(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            DbItem parent,
            DbItem child1,
            DbItem child2,
            CallContext context)
        {
            sut.DataStorage.GetFakeItem(itemDefinition.ID).Returns(parent);
            parent.Children.Add(child1);
            parent.Children.Add(child2);
            var expected = new IDList { child1.ID, child2.ID };

            sut.GetChildIDs(itemDefinition, context).ShouldBeEquivalentTo(expected);
        }

        [Theory, DefaultAutoData]
        public void GetParendIdThrowsIfItemDefinitionIsNull([Greedy] FakeDataProvider sut, CallContext context)
        {
            Action action = () => sut.GetParentID(null, context);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*itemDefinition");
        }

        [Theory, DefaultAutoData]
        public void GetParendIdReturnsParentId(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            DbItem item,
            CallContext context)
        {
            sut.DataStorage.GetFakeItem(itemDefinition.ID).Returns(item);
            var result = sut.GetParentID(itemDefinition, context);
            result.Should().Be(item.ParentID);
        }

        [Theory, DefaultAutoData]
        public void GetParendIdReturnsNullIfNoItemFound(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            CallContext context)
        {
            var result = sut.GetParentID(itemDefinition, context);
            result.Should().BeNull();
        }

        [Theory, DefaultAutoData]
        public void GetParendIdReturnsNullForSitecoreRootItem(
            [Greedy] FakeDataProvider sut,
            string itemName,
            ID id,
            ID templateId,
            CallContext context)
        {
            var itemDefinition = new ItemDefinition(ItemIDs.RootID, itemName, id, templateId);
            var result = sut.GetParentID(itemDefinition, context);
            result.Should().BeNull();
        }

        [Theory, DefaultAutoData]
        public void ShouldGetTemplateIds([Greedy] FakeDataProvider sut, DbTemplate template)
        {
            sut.DataStorage.GetFakeTemplates().Returns(new[] { template });
            sut.GetTemplateItemIds(null).Should().Contain(template.ID);
        }

        [Theory, DefaultAutoData]
        public void ShouldGetTemplatesFromDataStorage([Greedy] FakeDataProvider sut, DbTemplate template)
        {
            sut.DataStorage.GetFakeTemplates().Returns(new[] { template });
            sut.GetTemplates(null).Should().HaveCount(1);
        }

        [Theory, DefaultAutoData]
        public void ShouldGetTemplatesWithDefaultDataSectionFromDataStorage([Greedy] FakeDataProvider sut, DbTemplate template)
        {
            sut.DataStorage.GetFakeTemplates().Returns(new[] { template });

            var result = sut.GetTemplates(null).First();

            result.GetSection("Data").Should().NotBeNull();
        }

        [Theory, DefaultAutoData]
        public void ShouldHaveStandardBaseTemplate([Greedy] FakeDataProvider sut, [NoAutoProperties] DbTemplate template)
        {
            sut.DataStorage.GetFakeTemplates().Returns(new[] { template });

            var result = sut.GetTemplates(null).First();

            result.BaseIDs.Single().Should().Be(TemplateIDs.StandardTemplate);
        }

        [Theory, DefaultAutoData]
        public void ShouldGetTemplateFields([Greedy] FakeDataProvider sut, DbTemplate template)
        {
            sut.DataStorage.GetFakeTemplates().Returns(new[] { template });
            template.Fields.Add("Title");

            var result = sut.GetTemplates(null).First();

            result.GetField("Title").Should().NotBeNull();
        }

        [Theory, DefaultAutoData]
        public void ShouldGetTemplateFieldType([Greedy] FakeDataProvider sut, DbTemplate template)
        {
            sut.DataStorage.GetFakeTemplates().Returns(new[] { template });
            template.Fields.Add(new DbField("Link") { Type = "General Link" });

            var result = sut.GetTemplates(null).First();

            result.GetField("Link").Type.Should().Be("General Link");
        }

        [Theory, DefaultAutoData]
        public void ShouldGetTemplateFieldIsShared([Greedy] FakeDataProvider sut, DbTemplate template)
        {
            sut.DataStorage.GetFakeTemplates().Returns(new[] { template });
            template.Fields.Add(new DbField("Title") { Shared = true });

            var result = sut.GetTemplates(null).First();

            result.GetField("Title").IsShared.Should().BeTrue();
        }

        [Theory, DefaultAutoData]
        public void ShouldGetTemplateFieldSource([Greedy] FakeDataProvider sut, DbTemplate template)
        {
            sut.DataStorage.GetFakeTemplates().Returns(new[] { template });
            template.Fields.Add(new DbField("Multilist") { Source = "/sitecore/content" });

            var result = sut.GetTemplates(null).First();

            result.GetField("Multilist").Source.Should().Be("/sitecore/content");
        }

        [Theory, DefaultAutoData]
        public void ShouldGetDefaultLanguage([Greedy] FakeDataProvider sut, CallContext context)
        {
            var langs = sut.GetLanguages(context);
            langs.Should().BeEmpty();
        }

        [Theory, DefaultAutoData]
        public void ShouldGetItemDefinition([Greedy] FakeDataProvider sut, DbItem item, CallContext context)
        {
            // arrange
            sut.DataStorage.GetFakeItem(item.ID).Returns(item);

            // act
            var definition = sut.GetItemDefinition(item.ID, context);

            // assert
            definition.ID.Should().Be(item.ID);
            definition.Name.Should().Be(item.Name);
            definition.TemplateID.Should().Be(item.TemplateID);
            definition.BranchId.Should().Be(ID.Null);
        }

        [Theory, DefaultAutoData]
        public void ShouldGetNullItemDefinitionIfNoItemFound([Greedy] FakeDataProvider sut, ID itemId, CallContext context)
        {
            sut.GetItemDefinition(itemId, context).Should().BeNull();
        }

        [Theory, DefaultAutoData]
        public void ShouldGetAllThePossibleItemVersions([Greedy] FakeDataProvider sut, ItemDefinition def, CallContext context)
        {
            // arrange
            var item = new DbItem("home", def.ID, def.TemplateID)
            {
                Fields =
                        {
                            new DbField("Field 1") {{"en", 1, string.Empty}, {"en", 2, string.Empty}, {"da", 1, string.Empty}},
                            new DbField("Field 2") {{"en", 1, string.Empty}, {"da", 1, string.Empty}, {"da", 2, string.Empty}}
                        }
            };

            sut.DataStorage.GetFakeItem(def.ID).Returns(item);

            // act
            var versions = sut.GetItemVersions(def, context);

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
        public void ShouldGetEmptyVersionsIfNoFakeItemFound([Greedy] FakeDataProvider sut, ItemDefinition def, CallContext context)
        {
            sut.GetItemVersions(def, context).Should().BeEmpty();
        }

        [Obsolete]
        [Theory, DefaultAutoData]
        public void ShouldSetPropertyAndReturnTrue(FakeDataProvider sut, string name, string value, CallContext context)
        {
            sut.SetProperty(name, value, context).Should().BeTrue();
        }

        [Obsolete]
        [Theory, DefaultAutoData]
        public void ShouldThrowIfNameIsNullOnSetProperty(FakeDataProvider sut)
        {
            Action action = () => sut.SetProperty(null, null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*name");
        }

        [Obsolete]
        [Theory, DefaultAutoData]
        public void ShouldGetProperty(FakeDataProvider sut, string name, string value, CallContext context)
        {
            sut.SetProperty(name, value, context);
            sut.GetProperty(name, context).Should().Be(value);
        }

        [Obsolete]
        [Theory, DefaultAutoData]
        public void ShouldThrowIfNameIsNullOnGetProperty(FakeDataProvider sut)
        {
            Action action = () => sut.GetProperty(null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*name");
        }

        [Obsolete]
        [Theory, DefaultAutoData]
        public void ShouldReturnNullIfNoPropertySet(FakeDataProvider sut, string name, CallContext context)
        {
            sut.GetProperty(name, context).Should().BeNull();
        }

        [Obsolete]
        [Theory, DefaultAutoData]
        public void ShouldResetPropertyAndReturnTheLatestValue(FakeDataProvider sut, string name, string value1, string value2, CallContext context)
        {
            sut.SetProperty(name, value1, context);
            sut.SetProperty(name, value2, context);
            sut.GetProperty(name, context).Should().Be(value2);
        }

        [Theory, DefaultAutoData]
        public void ShouldGetNullItemFieldsIfNoItemFound([Greedy] FakeDataProvider sut, ItemDefinition def, VersionUri versionUri, CallContext context)
        {
            sut.GetItemFields(def, versionUri, context).Should().BeNull();
        }

        [Theory, DefaultAutoData]
        public void ShouldGetItemFields([Greedy] FakeDataProvider sut, DbTemplate template, DbItem item, DbField field, Language language, Version version,
            CallContext context)
        {
            template.Fields.Add(field);
            item.Fields.Add(field); // ?
            item.TemplateID = template.ID;

            sut.DataStorage.GetFakeTemplate(template.ID).Returns(template);
            sut.DataStorage.GetFakeItem(item.ID).Returns(item);

            var def = new ItemDefinition(item.ID, item.Name, item.TemplateID, item.BranchId);
            var versionUri = new VersionUri(language, version);

            sut.GetItemFields(def, versionUri, context).Should().HaveCount(1);
        }

        [Theory, DefaultAutoData]
        public void AddToPublishQueueReturnsTrue(
            [Greedy] FakeDataProvider sut,
            ID itemId,
            string action,
            DateTime date,
            CallContext context)
        {
            sut.AddToPublishQueue(itemId, action, date, context).Should().BeTrue();
        }

#if !SC80160115 // Missing in 8.0
        [Theory, DefaultAutoData]
        public void AddToPublishQueueWithLanguageReturnsTrue(
            [Greedy] FakeDataProvider sut,
            ID itemId,
            string action,
            DateTime date,
            string language,
            CallContext context)
        {
            sut.AddToPublishQueue(itemId, action, date, language, context).Should().BeTrue();
        }
#endif

        [Theory, DefaultAutoData]
        public void AddToPublishQueueSameItemIdMultipleTimesReturnsTrue(
            [Greedy] FakeDataProvider sut,
            ID itemId,
            string action,
            DateTime date,
            CallContext context)
        {
            sut.AddToPublishQueue(itemId, action, date, context).Should().BeTrue();
            sut.AddToPublishQueue(itemId, action, date, context).Should().BeTrue();
            sut.AddToPublishQueue(itemId, action, date, context).Should().BeTrue();
        }

        [Theory, DefaultAutoData]
        public void GetPublishQueueReturnsIdList(
            [Greedy] FakeDataProvider sut,
            ID itemId1,
            ID itemId2,
            string action,
            DateTime date,
            CallContext context)
        {
            sut.AddToPublishQueue(itemId1, action, date, context);
            sut.AddToPublishQueue(itemId2, action, date, context);
            var result = sut.GetPublishQueue(DateTime.MinValue, DateTime.MaxValue, context);
            result.ShouldBeEquivalentTo(new IDList { itemId1, itemId2 });
        }

        [Theory, DefaultAutoData]
        public void GetPublishQueueReturnsEmptyIdListIfNoItemsAdded(
            [Greedy] FakeDataProvider sut,
            CallContext context)
        {
            var result = sut.GetPublishQueue(DateTime.MinValue, DateTime.MaxValue, context);
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineDefaultAutoData(-1, 0, 1)]
        [InlineDefaultAutoData(0, 0, 1)]
        [InlineDefaultAutoData(0, 1, 1)]
        [InlineDefaultAutoData(1, 2, 0)]
        [InlineDefaultAutoData(-2, -1, 0)]
        public void GetPublishQueueReturnsIdListFilteredByDates(
            int daysBeforePublishingDate,
            int daysAfterPublishingDate,
            int expectedCount,
            [Greedy] FakeDataProvider sut,
            ID itemId,
            string action,
            DateTime date,
            CallContext context)
        {
            var from = date.Add(TimeSpan.FromDays(daysBeforePublishingDate));
            var to = date.Add(TimeSpan.FromDays(daysAfterPublishingDate));
            sut.AddToPublishQueue(itemId, action, date, context);

            var result = sut.GetPublishQueue(from, to, context);

            result.Count.Should().Be(expectedCount);
        }

        [Theory, DefaultAutoData]
        public void GetPublishQueueReturnsIdListWithoutDuplicatedIDs(
            [Greedy] FakeDataProvider sut,
            ID itemId,
            string action,
            DateTime date,
            CallContext context)
        {
            sut.AddToPublishQueue(itemId, action, date, context);
            sut.AddToPublishQueue(itemId, action, date, context);
            var result = sut.GetPublishQueue(DateTime.MinValue, DateTime.MaxValue, context);
            result.ShouldBeEquivalentTo(new IDList { itemId });
        }

        [Theory, DefaultAutoData]
        public void MoveItemThrowsIfItemDefinitionIsNull(FakeDataProvider sut, ItemDefinition destination)
        {
            Action action = () => sut.MoveItem(null, destination, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*itemDefinition");
        }

        [Theory, DefaultAutoData]
        public void MoveItemThrowsIfDestinationIsNull(FakeDataProvider sut, ItemDefinition itemDefinition)
        {
            Action action = () => sut.MoveItem(itemDefinition, null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*destination");
        }

        [Theory, DefaultAutoData]
        public void MoveItemThrowsIfItemDefinitionNotFound(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            ItemDefinition destination)
        {
            Action action = () => sut.MoveItem(itemDefinition, destination, null);
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Unable to move item. The item '{0}' is not found.".FormatWith(itemDefinition.ID));
        }

        [Theory, DefaultAutoData]
        public void MoveItemThrowsIfDestinationNotFound(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            ItemDefinition destination,
            DbItem item)
        {
            sut.DataStorage.GetFakeItem(itemDefinition.ID).Returns(item);
            Action action = () => sut.MoveItem(itemDefinition, destination, null);
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Unable to move item. The destination item '{0}' is not found.".FormatWith(destination.ID));
        }

        [Theory, DefaultAutoData]
        public void MoveItemRemovesFromOldParentChildrenIfExists(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            ItemDefinition destination,
            DbItem item,
            DbItem newDestination,
            DbItem oldParent)
        {
            oldParent.Children.Add(item);
            sut.DataStorage.GetFakeItem(itemDefinition.ID).Returns(item);
            sut.DataStorage.GetFakeItem(destination.ID).Returns(newDestination);
            sut.DataStorage.GetFakeItem(item.ParentID).Returns(oldParent);

            sut.MoveItem(itemDefinition, destination, null);

            oldParent.Children.Should().BeEmpty();
        }

        [Theory, DefaultAutoData]
        public void MoveItemToNewDestinationReturnsTrue(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            ItemDefinition destination,
            DbItem item,
            DbItem newDestination)
        {
            sut.DataStorage.GetFakeItem(itemDefinition.ID).Returns(item);
            sut.DataStorage.GetFakeItem(destination.ID).Returns(newDestination);

            sut.MoveItem(itemDefinition, destination, null).Should().BeTrue();

            newDestination.Children.Single().Should().BeSameAs(item);
            item.ParentID.Should().Be(newDestination.ID);
        }

        [Theory, DefaultAutoData]
        public void ShouldSetBlobStreamInDataStorage(
            [Greedy] FakeDataProvider sut,
            Guid blobId,
            [Modest] MemoryStream stream,
            CallContext context)
        {
            sut.SetBlobStream(stream, blobId, context);
            sut.DataStorage.Received().SetBlobStream(blobId, stream);
        }

        [Theory]
        [InlineDefaultAutoData("/sitecore/content/home")]
        [InlineDefaultAutoData("/Sitecore/Content/Home")]
        [InlineDefaultAutoData("/Sitecore/Content/Home/")]
        public void ShouldResolvePath(string path, [Greedy] FakeDataProvider sut, DbItem item, CallContext context)
        {
            item.FullPath = "/sitecore/content/home";
            sut.DataStorage.GetFakeItems().Returns(new[] { item });

            sut.ResolvePath(path, context).Should().Be(item.ID);
        }

        [Theory, DefaultAutoData]
        public void ShouldReturnNullIfNoItemFound([Greedy] FakeDataProvider sut, string path, CallContext context)
        {
            sut.ResolvePath(path, context).Should().BeNull();
        }

        [Theory, DefaultAutoData]
        public void ShouldReturnIdIfPathIsId([Greedy] FakeDataProvider sut, ID itemId, CallContext context)
        {
            sut.ResolvePath(itemId.ToString(), context).Should().Be(itemId);
        }

        [Theory, DefaultAutoData]
        public void ShouldResolveFirstItemId(
            [Greedy] FakeDataProvider sut,
            DbItem item1,
            DbItem item2,
            CallContext context)
        {
            const string path = "/sitecore/content/home";
            item1.FullPath = path;
            item2.FullPath = path;
            sut.DataStorage.GetFakeItems().Returns(new[] { item1, item2 });

            sut.ResolvePath(path, context).Should().Be(item1.ID);
        }

        [Theory, DefaultAutoData]
        public void RemoveVersionThrowsIfItemDefinitionIsNull(FakeDataProvider sut, VersionUri baseVersion)
        {
            Action action = () => sut.RemoveVersion(null, baseVersion, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*itemDefinition");
        }

        [Theory, DefaultAutoData]
        public void RemoveVersionThrowsIfBaseVersionIsNull(FakeDataProvider sut, ItemDefinition itemDefinition)
        {
            Action action = () => sut.RemoveVersion(itemDefinition, null, null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*version");
        }

        [Theory, DefaultAutoData]
        public void RemoveVersionThrowsIfItemDefinitionNotFound(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            VersionUri baseVersion)
        {
            Action action = () => sut.RemoveVersion(itemDefinition, baseVersion, null);
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Unable to remove item version. The item '{0}' is not found.".FormatWith(itemDefinition.ID));
        }

        [Theory, DefaultAutoData]
        public void RemoveVersionReturnsFalseIfNoVersionFound(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            VersionUri version,
            DbItem item)
        {
            sut.DataStorage.GetFakeItem(itemDefinition.ID).Returns(item);
            sut.RemoveVersion(itemDefinition, version, null).Should().BeFalse();
        }

        [Theory, DefaultAutoData]
        public void RemoveVersionRemovesVersionAndReturnsTrue(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            Language language,
            DbItem item)
        {
            sut.DataStorage.GetFakeItem(itemDefinition.ID).Returns(item);
            item.AddVersion(language.Name);
            item.AddVersion(language.Name);
            var version = new VersionUri(language, Version.Latest);
            var expectedVersionCount = 1;

            var result = sut.RemoveVersion(itemDefinition, version, null);

            result.Should().BeTrue();
            item.GetVersionCount(language.Name).Should().Be(expectedVersionCount);
        }

        [Theory, DefaultAutoData]
        public void SaveItemThrowsIfItemDefinitionIsNull(FakeDataProvider sut)
        {
            Action action = () => sut.SaveItem(null, null, null);
            action.ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("itemDefinition");
        }

        [Theory, DefaultAutoData]
        public void SaveItemThrowsIfItemChangesParameterIsNull(
            FakeDataProvider sut,
            ItemDefinition itemDefinition)
        {
            Action action = () => sut.SaveItem(itemDefinition, null, null);
            action.ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("changes");
        }

        [Theory, DefaultAutoData]
        public void SaveItemReturnsFalse(
            [Greedy] FakeDataProvider sut,
            ItemDefinition itemDefinition,
            ItemChanges changes,
            CallContext context)
        {
            // 'true' looks more correct here but numerous tests start failing then.
            sut.SaveItem(itemDefinition, changes, context).Should().BeFalse();
        }
    }
}