using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Sitecore.Data;
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

        [Fact]
        public void ShouldDeserializeDescendants()
        {
            DsDbItem item = new DsDbItem("/sitecore/content/home", true);

            item.Should().NotBeNull();

            DbItem child = item.Children.FirstOrDefault(c => c.Name == "Child Item");
            child.Should().NotBeNull();
            child.Name.Should().BeEquivalentTo("Child Item");

            DbItem grandChild = child.Children.FirstOrDefault(c => c.Name == "Grandchild Item");
            grandChild.Should().NotBeNull();
            grandChild.Name.Should().BeEquivalentTo("Grandchild Item");
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
    }
}
