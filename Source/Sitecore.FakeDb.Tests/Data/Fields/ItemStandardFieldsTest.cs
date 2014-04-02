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

    public ItemStandardFieldsTest()
    {
      // arrange
      item = new DbItem("item");
      template = new DbTemplate("template");
    }

    [Fact]
    public void ShouldHaveLayoutField()
    {
      // assert
      item.Fields[FieldIDs.LayoutField].Should().NotBeNull();
      template.Fields[FieldIDs.LayoutField].Should().NotBeNull();

    }

    [Fact]
    public void ShouldHaveStandardValuesField()
    {
      item.Fields[FieldIDs.StandardValues].Should().NotBeNull();
      template.Fields[FieldIDs.StandardValues].Should().NotBeNull();
    }

    [Fact]
    public void TemplateAndNotItemShouldHaveBaseTemplatesField()
    {
      item.Fields[FieldIDs.BaseTemplate].Should().BeNull();
      template.Fields[FieldIDs.BaseTemplate].Should().NotBeNull();
    }
  }
}
