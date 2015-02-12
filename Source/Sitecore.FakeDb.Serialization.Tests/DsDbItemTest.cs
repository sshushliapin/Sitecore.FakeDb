using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Xunit;

namespace Sitecore.FakeDb.Serialization.Tests
{
    public class DsDbItemTest
    {
        [Fact]
        public void ShouldSetName()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/home");

            item.Should().NotBeNull();
            item.Name.Should().BeEquivalentTo("Home");
        }

        [Fact]
        public void ShouldSetItemId()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/home");

            item.Should().NotBeNull();
            item.ID.ShouldBeEquivalentTo(ID.Parse("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"));
        }

        [Fact]
        public void ShouldSetTemplateId()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/home");

            item.Should().NotBeNull();
            item.TemplateID.ShouldBeEquivalentTo(ID.Parse("{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}"));
        }

        [Fact]
        public void ShouldDeserializeMultilingualItem()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/home");

            using (new Db()
                {
                    item
                })
            {
                item.Should().NotBeNull();

                ID titleFieldId = ID.Parse("{75577384-3C97-45DA-A847-81B00500E250}");
                ID sharedFieldId = ID.Parse("{8F0BDC2B-1A78-4C29-BF83-C1C318186AA6}");

                item.Fields[titleFieldId].Value.Should().BeEquivalentTo("Sitecore");
                item.Fields[sharedFieldId].Value.Should()
                    .BeEquivalentTo("This value should be the same in all languages");

                using (new LanguageSwitcher("de-DE"))
                {
                    item.Fields[titleFieldId].Value.Should().BeEquivalentTo("Sitekern");
                    item.Fields[sharedFieldId].Value.Should()
                        .BeEquivalentTo("This value should be the same in all languages");
                }
            }
        }

        [Fact]
        public void ShouldDeserializeDescendants()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/home", true);
            using (new Db()
                {
                    item
                })
            {
                item.Should().NotBeNull();

                DbItem child = item.Children.FirstOrDefault(c => c.Name == "Child Item");
                child.Should().NotBeNull();
                child.Name.Should().BeEquivalentTo("Child Item");

                DbItem grandChild = child.Children.FirstOrDefault(c => c.Name == "Grandchild Item");
                grandChild.Should().NotBeNull();
                grandChild.Name.Should().BeEquivalentTo("Grandchild Item");
            }
        }

        [Fact]
        public void ShouldLoadFromSpecifiedFolder()
        {
            DsDbItem item = new DsDbItem(
                "/sitecore/content/Item only available in custom serialization folder",
                "custom");

            item.Should().NotBeNull();
        }

        [Fact]
        public void ShouldNotLoadFromWrongFolder()
        {
            Action createItem = () => new DsDbItem("/sitecore/content/Item only available in custom serialization folder");

            createItem.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ShouldDetermineFolderFromContextDatabase()
        {
            using (new DatabaseSwitcher(Database.GetDatabase("core")))
            {
                DsDbItem item = new DsDbItem("/sitecore/content/Applications");

                item.Should().NotBeNull();
                item.Name.ShouldBeEquivalentTo("Applications");
            }
        }

        [Fact]
        public void ShouldLookupById()
        {
            ID id = ID.Parse("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}");

            DsDbItem item = new DsDbItem(id);

            item.Should().NotBeNull();
            item.Name.Should().BeEquivalentTo("Home");
            item.ID.ShouldBeEquivalentTo(id);
        }

        [Fact]
        public void ShouldAutoDeserializeLinkedTemplate()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/home");
            using (Db db = new Db()
                {
                    item
                })
            {
                ID templateId = ID.Parse("{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}");

                item.Should().NotBeNull();
                item.TemplateID.ShouldBeEquivalentTo(templateId);

                Item templateItem = db.GetItem(item.TemplateID);

                templateItem.Should().NotBeNull();
                templateItem.Name.ShouldBeEquivalentTo("Sample Item");
            }
        }

        [Fact]
        public void ShouldNotAutoDeserializeLinkedTemplate()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/home", false, false);
            using (Db db = new Db()
                {
                    item
                })
            {
                item.Should().NotBeNull();

                Item templateItem = db.GetItem(item.TemplateID);
                templateItem.Name.Should().NotBe("Sample Item");
            }
        }

        [Fact]
        public void ShouldAutoDeserializeIncludingBaseTemplates()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/Home/Some item");
            using (Db db = new Db()
                {
                    item
                })
            {
                ID templateId = ID.Parse("{F6A72DBF-558F-40E5-8033-EE4ACF027FE2}");
                ID baseTemplateId = ID.Parse("{C4F448EB-7CA3-4A27-BA5E-99E8B9022803}");

                item.Should().NotBeNull();
                item.TemplateID.ShouldBeEquivalentTo(templateId);

                Item templateItem = db.GetItem(item.TemplateID);

                templateItem.Should().NotBeNull();
                templateItem.Name.ShouldBeEquivalentTo("Some template");
                templateItem[FieldIDs.BaseTemplate].ShouldBeEquivalentTo(baseTemplateId.ToString());

                Item baseTemplateItem = db.GetItem(baseTemplateId);

                baseTemplateItem.Should().NotBeNull();
                baseTemplateItem.Name.ShouldBeEquivalentTo("Some base template");
                baseTemplateItem.ID.ShouldBeEquivalentTo(baseTemplateId);
            }
        }

        [Fact]
        public void ShouldDeserializeShortenedPath()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/this/path must be very deep/so that it will be shortened/on the filesystem/abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz/still deeper");

            item.Should().NotBeNull();
        }

        [Fact]
        public void ShouldDeserializeShortenedPathOneDirectoryHigher()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/this/path must be very deep/so that it will be shortened/on the filesystem/abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz");

            item.Should().NotBeNull();
        }

        [Fact]
        public void ShouldLookupByIdInShortenedPath()
        {
            ID id = ID.Parse("{9897418C-9D6A-4AC9-B08A-15064C9582A9}");

            DsDbItem item = new DsDbItem(id);

            item.Should().NotBeNull();
            item.Name.Should().BeEquivalentTo("still deeper");
            item.ID.ShouldBeEquivalentTo(id);
        }
    }
}
