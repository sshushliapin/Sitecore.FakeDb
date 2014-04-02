using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Sitecore.Data.Fields;
using Sitecore.Data.Templates;
using Xunit;

namespace Sitecore.FakeDb.Tests.Data.Fields
{
  public class ItemStandardFieldsTest
  {
    private DbItem item;
    private DbTemplate template;


    [Fact]
    public void ShouldHaveLayoutField()
    {
      using (var db = new Db
      {
        new DbItem("item1"),
        new DbTemplate("template1")
      })
      {
        var item = db.GetItem("/sitecore/content/item1");
        var template = db.GetItem("/sitecore/templates/template1");

        // assert
        item.Fields[FieldIDs.LayoutField].Should().NotBeNull();
        template.Fields[FieldIDs.LayoutField].Should().NotBeNull();
      }
    }

    [Fact]
    public void ShouldHaveStandardValuesField()
    {
      using (var db = new Db
      {
        new DbTemplate("template1")
      })
      {
        var item = db.GetItem("/sitecore/content/item1");
        var template = db.GetItem("/sitecore/templates/template1");

        // assert
        template.Fields[FieldIDs.StandardValues].Should().NotBeNull();
      }
      
    }

    [Fact]
    public void TemplateAndNotItemShouldHaveBaseTemplatesField()
    {
      using (var db = new Db
      {
        new DbItem("item1"),
        new DbTemplate("template1")
      })
      {
        var item = db.GetItem("/sitecore/content/item1");
        var template = db.GetItem("/sitecore/templates/template1");

        template.Fields[FieldIDs.BaseTemplate].Should().NotBeNull();
        template.Fields[FieldIDs.BaseTemplate].Value.Should().BeEquivalentTo(TemplateIDs.StandardTemplate.ToString());

        // __Base tempalte is a field on the DbTemplate so it will "show through" but it should have no value in it
        item.Fields[FieldIDs.BaseTemplate].Value.Should().BeEmpty();
      }
    }
  }
}
