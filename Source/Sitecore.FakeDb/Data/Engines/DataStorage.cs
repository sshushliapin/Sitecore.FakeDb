using System.Linq;
using Lucene.Net.Documents;
using Sitecore.Data.Templates;
using Sitecore.Shell.Framework.Commands.Favorites;

namespace Sitecore.FakeDb.Data.Engines
{
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;

  public class DataStorage
  {
    private static readonly ID RootTemplateId = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private const string SitecoreItemName = "sitecore";

    private const string ContentItemName = "content";

    private const string TemplatesItemName = "templates";

    public const string TemplateItemName = "Template";

    public const string TemplateSectionItemName = "Template section";

    public const string TemplateFieldItemName = "Template field";

    public const string BranchItemName = "Branch";

    public const string StandardTemplateName = "Standard template";

    public static ID LayoutSectionTemplateId = new ID("{4D30906D-0B49-4FA7-969D-BF90157357EA}");

    public static ID AdvancedSectionTemplateId = new ID("{646F4B34-708C-41C2-9F4B-2661849777F3}");

    public const string StandardValuesFieldName = "__Standard values";

    public const string BaseTemplateFieldName = "__Base template";

    public const string LayoutDetailsFieldName = "__Renderings";

    private Database database;

    public DataStorage(Database database)
    {
      this.database = database;

      this.FakeItems = new Dictionary<ID, DbItem>();
      this.FakeTemplates = new Dictionary<ID, DbTemplate>();

      this.FillDefaultFakeItems();
    }

    public Database Database
    {
      get { return this.database; }
    }

    /// <summary>
    /// Gets the fake items.
    /// </summary>
    public IDictionary<ID, DbItem> FakeItems { get; private set; }

    public IDictionary<ID, DbTemplate> FakeTemplates { get; private set; }

    public virtual DbItem GetFakeItem(ID itemId)
    {
      Assert.ArgumentCondition(!ID.IsNullOrEmpty(itemId), "itemId", "Value cannot be null.");

      return this.FakeItems.ContainsKey(itemId) ? this.FakeItems[itemId] : null;
    }

    public virtual DbTemplate GetFakeTemplate(ID templateId)
    {
      Assert.ArgumentCondition(!ID.IsNullOrEmpty(templateId), "templateId", "Value cannot be null.");

      return this.FakeTemplates.ContainsKey(templateId) ? this.FakeTemplates[templateId] : null;
    }

    public virtual FieldList GetFieldList(ID templateId)
    {
      return GetFieldList(templateId, false);
    }

    public virtual FieldList GetFieldList(ID templateId, bool forItemData)
    {
      Assert.ArgumentCondition(!ID.IsNullOrEmpty(templateId), "templateId", "Value cannot be null.");

      var template = this.GetFakeTemplate(templateId);
      Assert.IsNotNull(template, "Template '{0}' not found.", templateId);

      var fieldList = new FieldList();
      if (forItemData)
      {
        foreach (var field in template.Fields)
        {
          // ItemData should only have fields that the item itself "carries". The other ones have to be null for Standard Values logic to kick in properly
          // ToDo: we should instead walk those standard template sections and not hardcode
          if (field.ID == FieldIDs.BaseTemplate || field.ID == FieldIDs.LayoutField)
          {
            continue;
          }

          fieldList.Add(field.ID, string.Empty);
        }
      }
      else
      {
        GetFieldListRecursively(template, fieldList);  
      }

      return fieldList;
    }

    private void GetFieldListRecursively(DbTemplate template, FieldList list)
    {
      var baseTemplates = template.Fields[FieldIDs.BaseTemplate].Value;
      if (!string.IsNullOrEmpty(baseTemplates)) {
        foreach (var baseTemplateId in baseTemplates.Split('|').Select(t => new ID(t)))
        {
          var baseTemplate = GetFakeTemplate(baseTemplateId);
          GetFieldListRecursively(baseTemplate, list);
        }
      }

      foreach (var field in template.Fields)
      {
        if (field.ID == FieldIDs.BaseTemplate) continue;

        list.Add(field.ID, string.Empty);
      }
    }

    public virtual Item GetSitecoreItem(ID itemId, Language language)
    {
      return this.GetSitecoreItem(itemId, language, Version.First);
    }

    public virtual Item GetSitecoreItem(ID itemId, Language language, Version version)
    {
      if (!this.FakeItems.ContainsKey(itemId))
      {
        return null;
      }

      var fakeItem = this.FakeItems[itemId];

      var itemVersion = version == Version.Latest ? Version.First : version;

      var fields = new FieldList();
      if (this.FakeTemplates.ContainsKey(fakeItem.TemplateID))
      {
        fields = this.GetFieldList(fakeItem.TemplateID, true);
      }

      foreach (var field in fakeItem.Fields)
      {
        var value = field.GetValue(language.Name, version.Number);
        fields.Add(field.ID, value);
      }

      var item = ItemHelper.CreateInstance(fakeItem.Name, fakeItem.ID, fakeItem.TemplateID, fields, this.database, language, itemVersion);

      return item;
    }

    protected virtual void FillDefaultFakeTemplate(DbTemplate template)
    {
      this.FakeItems.Add(template.ID, template);
      this.FakeTemplates.Add(template.ID, template);
    }

    protected virtual void FillDefaultFakeItems()
    {
      this.FakeItems.Add(ItemIDs.RootID, new DbItem(SitecoreItemName, ItemIDs.RootID, RootTemplateId) { ParentID = ID.Null, FullPath = "/sitecore" });
      this.FakeItems.Add(ItemIDs.ContentRoot, new DbItem(ContentItemName, ItemIDs.ContentRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/content" });
      this.FakeItems.Add(ItemIDs.TemplateRoot, new DbItem(TemplatesItemName, ItemIDs.TemplateRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/templates" });

      //ToDo: it would be more appropriate to create these templates in the /sitecore/templates/System/...

      FillDefaultFakeTemplate(new DbTemplate(TemplateItemName, TemplateIDs.Template, new ID[] { }) { ParentID = ItemIDs.TemplateRoot,  FullPath = "/sitecore/templates/template" });
      FillDefaultFakeTemplate(new DbTemplate(TemplateSectionItemName, TemplateIDs.TemplateSection, new ID[] { }) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template section" });
      FillDefaultFakeTemplate(new DbTemplate(TemplateFieldItemName, TemplateIDs.TemplateField, new ID[] { }) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template field" });
      FillDefaultFakeTemplate(new DbTemplate(BranchItemName, TemplateIDs.BranchTemplate, new ID[] { }) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/branch" });
      FillDefaultFakeTemplate(new DbTemplate("Layout", LayoutSectionTemplateId, new ID[] { })
      {
        ParentID = ItemIDs.TemplateRoot,
        FullPath = "/sitecore/templates/layout",
        Fields =
        {
          new DbField(FieldIDs.LayoutField)          
        }
      });
      FillDefaultFakeTemplate(new DbTemplate("Advanced", AdvancedSectionTemplateId, new ID[] { })
      {
        ParentID = ItemIDs.TemplateRoot,
        FullPath = "/sitecore/templates/advanced",
        Fields =
        {
          new DbField(FieldIDs.StandardValues)
        }
      });

      FillDefaultFakeTemplate(new DbTemplate(StandardTemplateName, TemplateIDs.StandardTemplate, new ID[] { LayoutSectionTemplateId, AdvancedSectionTemplateId })
      {
        ParentID = ItemIDs.TemplateRoot, 
        FullPath = "/sitecore/templates/Standard template"
      });
    }

  }
}