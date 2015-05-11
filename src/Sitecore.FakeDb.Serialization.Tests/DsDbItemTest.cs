namespace Sitecore.FakeDb.Serialization.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Globalization;
  using Xunit;

  public class DsDbItemTest
  {
    [Fact]
    public void ShouldSetName()
    {
      var item = new DsDbItem("/sitecore/content/home");

      item.Should().NotBeNull();
      item.Name.Should().BeEquivalentTo("Home");
    }

    [Fact]
    public void ShouldSetItemId()
    {
      var item = new DsDbItem("/sitecore/content/home");

      item.Should().NotBeNull();
      item.ID.ShouldBeEquivalentTo(SerializationId.ContentHomeItem);
    }

    [Fact]
    public void ShouldSetTemplateId()
    {
      var item = new DsDbItem("/sitecore/content/home");

      item.Should().NotBeNull();
      item.TemplateID.ShouldBeEquivalentTo(SerializationId.SampleItemTemplate);
    }

    [Fact]
    public void ShouldDeserializeMultilingualItem()
    {
      var item = new DsDbItem("/sitecore/content/home");

      using (new Db { item })
      {
        item.Should().NotBeNull();

        var titleFieldId = ID.Parse("{75577384-3C97-45DA-A847-81B00500E250}");
        var sharedFieldId = ID.Parse("{8F0BDC2B-1A78-4C29-BF83-C1C318186AA6}");

        item.Fields[titleFieldId].Value.Should().BeEquivalentTo("Sitecore");
        item.Fields[sharedFieldId].Value.Should().BeEquivalentTo("This value should be the same in all languages");

        using (new LanguageSwitcher("de-DE"))
        {
          item.Fields[titleFieldId].Value.Should().BeEquivalentTo("Sitekern");
          item.Fields[sharedFieldId].Value.Should().BeEquivalentTo("This value should be the same in all languages");
        }
      }
    }

    [Fact]
    public void ShouldDeserializeDescendants()
    {
      var item = new DsDbItem("/sitecore/content/home", true);
      using (new Db { item })
      {
        item.Should().NotBeNull();

        var child = item.Children.FirstOrDefault(c => c.Name == "Child Item");
        child.Should().NotBeNull();
        child.Name.Should().BeEquivalentTo("Child Item");

        var grandChild = child.Children.FirstOrDefault(c => c.Name == "Grandchild Item");
        grandChild.Should().NotBeNull();
        grandChild.Name.Should().BeEquivalentTo("Grandchild Item");
      }
    }

    [Fact]
    public void ShouldLoadFromSpecifiedFolder()
    {
      var item = new DsDbItem("/sitecore/content/Item only available in custom serialization folder", "custom");

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
        var item = new DsDbItem("/sitecore/content/Applications");

        item.Should().NotBeNull();
        item.Name.ShouldBeEquivalentTo("Applications");
      }
    }

    [Fact]
    public void ShouldLookupById()
    {
      var id = SerializationId.ContentHomeItem;

      var item = new DsDbItem(id);

      item.Should().NotBeNull();
      item.Name.Should().BeEquivalentTo("Home");
      item.ID.ShouldBeEquivalentTo(id);
    }

    [Fact]
    public void ShouldAutoDeserializeLinkedTemplate()
    {
      var item = new DsDbItem("/sitecore/content/home");
      using (var db = new Db { item })
      {
        var templateId = SerializationId.SampleItemTemplate;

        item.Should().NotBeNull();
        item.TemplateID.ShouldBeEquivalentTo(templateId);

        var templateItem = db.GetItem(item.TemplateID);

        templateItem.Should().NotBeNull();
        templateItem.Name.ShouldBeEquivalentTo("Sample Item");
      }
    }

    [Fact]
    public void ShouldNotAutoDeserializeLinkedTemplate()
    {
      var item = new DsDbItem("/sitecore/content/home", false, false);
      using (var db = new Db { item })
      {
        item.Should().NotBeNull();

        var templateItem = db.GetItem(item.TemplateID);
        templateItem.Name.Should().NotBe("Sample Item");
      }
    }

    [Fact]
    public void ShouldAutoDeserializeIncludingBaseTemplates()
    {
      var item = new DsDbItem("/sitecore/content/Home/Some item");
      using (var db = new Db { item })
      {
        var templateId = ID.Parse("{F6A72DBF-558F-40E5-8033-EE4ACF027FE2}");
        var baseTemplateId = ID.Parse("{C4F448EB-7CA3-4A27-BA5E-99E8B9022803}");

        item.Should().NotBeNull();
        item.TemplateID.ShouldBeEquivalentTo(templateId);

        var templateItem = db.GetItem(item.TemplateID);

        templateItem.Should().NotBeNull();
        templateItem.Name.ShouldBeEquivalentTo("Some template");
        templateItem[FieldIDs.BaseTemplate].ShouldBeEquivalentTo(baseTemplateId.ToString());

        var baseTemplateItem = db.GetItem(baseTemplateId);

        baseTemplateItem.Should().NotBeNull();
        baseTemplateItem.Name.ShouldBeEquivalentTo("Some base template");
        baseTemplateItem.ID.ShouldBeEquivalentTo(baseTemplateId);
      }
    }

    [Fact]
    public void ShouldDeserializeShortenedPath()
    {
      var item = new DsDbItem("/sitecore/content/this/path must be very deep/so that it will be shortened/on the filesystem/abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz/still deeper");

      item.Should().NotBeNull();
    }

    [Fact]
    public void ShouldDeserializeShortenedPathOneDirectoryHigher()
    {
      var item = new DsDbItem("/sitecore/content/this/path must be very deep/so that it will be shortened/on the filesystem/abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz");

      item.Should().NotBeNull();
    }

    [Fact]
    public void ShouldLookupByIdInShortenedPath()
    {
      var id = ID.Parse("{9897418C-9D6A-4AC9-B08A-15064C9582A9}");

      var item = new DsDbItem(id);

      item.Should().NotBeNull();
      item.Name.Should().BeEquivalentTo("still deeper");
      item.ID.ShouldBeEquivalentTo(id);
    }

    [Fact]
    public void ShouldDeserializeItemBasedOnTwoTemplates()
    {
      // arrange
      using (var db = new Db
                        {
                          new DsDbItem("/sitecore/content/New Composite Sample Item")
                        })
      {
        // act
        var item = db.GetItem("/sitecore/content/New Composite Sample Item");

        // assert
        item["Sample Field One"].Should().Be("Value One");
        item["Sample Field Two"].Should().Be("Value Two");
      }
    }
  }
}