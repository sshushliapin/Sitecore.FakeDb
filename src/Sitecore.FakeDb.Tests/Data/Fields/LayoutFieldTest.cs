using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Xml.Patch;
using Xunit;

namespace Sitecore.FakeDb.Tests.Data.Fields
{
  public class LayoutFieldTest
  {
    [Fact]
    public void ShouldBeAbleToSetLayoutFieldValue()
    {
      ID itemId = ID.NewID;

      using (var db = new Db
      {
        new DbItem("page", itemId)
        {
          new DbField(FieldIDs.LayoutField)
          {
            Value = "<r/>"
          }
        }
      })
      {
        DbItem fakeItem = db.DataStorage.FakeItems[itemId];
        Assert.Equal("<r/>", fakeItem.Fields[FieldIDs.LayoutField].Value);

        var item = db.GetItem("/sitecore/content/page");

        var layoutField = item.Fields[FieldIDs.LayoutField];
        Assert.Equal("<r/>", layoutField.Value);

        Assert.Equal("<r/>", item[FieldIDs.LayoutField]);
        Assert.Equal("<r/>", item["__Renderings"]);

        Assert.Equal("<r/>", LayoutField.GetFieldValue(item.Fields[FieldIDs.LayoutField]));
      }
    }

    [Fact]
    public void ShouldWorkWithLayoutDeltas()
    {
      ID templateId = ID.NewID;

      string templateLayout =
        @"<r xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
            <d id=""{FE5D7FDF-89C0-4D99-9AA3-B5FBD009C9F3}"" l=""{4CA7478E-6184-4890-9072-1156DB468A1B}"" />
            <d id=""{EF7176AE-2502-401A-96C0-1E9918A982F7}"" l=""{4CA7478E-6184-4890-9072-1156DB468A1B}"" /> 
            <d id=""{5A6E7DC3-987F-4E74-AF78-AC0E544975F2}"" l=""{4CA7478E-6184-4890-9072-1156DB468A1B}"" />
          </r>";

      string itemDelta = 
        @"<r xmlns:p=""p"" xmlns:s=""s"" p:p=""1"">
              <d id=""{FE5D7FDF-89C0-4D99-9AA3-B5FBD009C9F3}"">
                  <r uid=""{BC2FDEAE-A971-420B-A874-BA5C767C42FE}"" s:id=""{B5BFA387-74C8-416B-98AF-01C9230C24B2}"" s:ph=""Score Content Main"" />
              </d>
          </r>";

      string merged = XmlDeltas.ApplyDelta(templateLayout, itemDelta);

      using (var db = new Db()
      {
        new DbTemplate("main", templateId)
        {
            { FieldIDs.LayoutField, templateLayout }
        },
        new DbItem("page", ID.NewID, templateId),
        new DbItem("page2", ID.NewID, templateId)
        {
          new DbField(FieldIDs.LayoutField) { Value = itemDelta }
        }
      })
      {
        Item item = db.GetItem("/sitecore/content/page");

        Assert.Equal(templateLayout, StandardValuesManager.Provider.GetStandardValue(item.Fields[FieldIDs.LayoutField]));

        Assert.Equal(templateLayout, LayoutField.GetFieldValue(item.Fields[FieldIDs.LayoutField]));
        // standard values
        Assert.Equal(templateLayout, item[FieldIDs.LayoutField]);

        var item2 = db.GetItem("/sitecore/content/page2");
        Assert.Equal(templateLayout, item2.Fields[FieldIDs.LayoutField].GetStandardValue());

        // just checking
        Assert.True(XmlPatchUtils.IsXmlPatch(itemDelta));

        Assert.Equal(merged, LayoutField.GetFieldValue(item2.Fields[FieldIDs.LayoutField]));
      }
    }
  }
}