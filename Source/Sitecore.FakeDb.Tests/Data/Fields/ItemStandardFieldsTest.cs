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
      item.StandardFields[FieldIDs.LayoutField].Should().NotBeNull();
      template.StandardFields[FieldIDs.LayoutField].Should().NotBeNull();

    }

    [Fact]
    public void ShouldHaveStandardValuesField()
    {
      item.StandardFields[FieldIDs.StandardValues].Should().NotBeNull();
      template.StandardFields[FieldIDs.StandardValues].Should().NotBeNull();
    }

    [Fact]
    public void TemplateAndNotItemShouldHaveBaseTemplatesField()
    {
      item.StandardFields[FieldIDs.BaseTemplate].Should().BeNull();
      template.StandardFields[FieldIDs.BaseTemplate].Should().NotBeNull();
    }
  }
}
