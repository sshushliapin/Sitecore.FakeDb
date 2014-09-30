namespace Sitecore.FakeDb.Data.Engines
{
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;
  using Version = Sitecore.Data.Version;

  public class DataStorage
  {
    private static readonly ID TemplateIdSitecore = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private const string SitecoreItemName = "sitecore";

    private const string ContentItemName = "content";

    private const string TemplatesItemName = "templates";

    private const string SystemItemName = "system";

    private const string MediaLibraryItemName = "media library";

    private readonly IDictionary<ID, DbItem> fakeItems;

    private readonly IDictionary<ID, DbTemplate> fakeTemplates;

    public const string TemplateItemName = "Template";

    public const string TemplateSectionItemName = "Template section";

    public const string TemplateFieldItemName = "Template field";

    public const string BranchItemName = "Branch";

    public const string FolderItemName = "Folder";

    private readonly Database database;

    public DataStorage(Database database)
    {
      this.database = database;

      this.fakeItems = new Dictionary<ID, DbItem>();
      this.fakeTemplates = new Dictionary<ID, DbTemplate>();

      this.FillDefaultFakeTemplates();
      this.FillDefaultFakeItems();
    }

    public Database Database
    {
      get { return this.database; }
    }

    public IDictionary<ID, DbItem> FakeItems
    {
      get { return this.fakeItems; }
    }

    public IDictionary<ID, DbTemplate> FakeTemplates
    {
      get { return this.fakeTemplates; }
    }

    public virtual void AddFakeTemplate(DbTemplate template)
    {
      this.FakeTemplates.Add(template.ID, template);

      this.Database.Engines.TemplateEngine.Reset();
    }

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
      return GetFieldList(templateId, null);
    }

    /// <summary>
    /// Returns all fields defined on the template with the provided ID. 
    /// 
    /// Does not (and should not) return a superset of fields across all base templates. 
    /// Sitecore does all necessary lookups when it needs to
    /// </summary>
    /// <param name="templateId"></param>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public virtual FieldList GetFieldList(ID templateId, string itemName)
    {
      Assert.ArgumentCondition(!ID.IsNullOrEmpty(templateId), "templateId", "Value cannot be null.");

      var template = this.GetFakeTemplate(templateId);
      Assert.IsNotNull(template, "Template '{0}' not found.", templateId);

      var fields = new FieldList();
      foreach (var field in template.Fields)
      {
        fields.Add(field.ID, string.Empty);
      }

      return fields;
    }

    public virtual Item GetSitecoreItem(ID itemId, Language language)
    {
      return this.GetSitecoreItem(itemId, language, Version.First);
    }

    public virtual Item GetSitecoreItem(ID itemId, Language language, Version version)
    {
      Assert.ArgumentNotNull(itemId, "itemId");
      Assert.ArgumentNotNull(language, "language");
      Assert.ArgumentNotNull(version, "version");

      if (!this.FakeItems.ContainsKey(itemId))
      {
        return null;
      }

      // TODO:[High] Avoid the templates resetting. Required to avoid sharing templates between unit tests.
      this.Database.Engines.TemplateEngine.Reset();

      var fakeItem = this.FakeItems[itemId];
      var itemVersion = version == Version.Latest ? Version.First : version;

      var fields = BuildItemFieldList(fakeItem, fakeItem.TemplateID, language, itemVersion);

      var item = ItemHelper.CreateInstance(fakeItem.Name, fakeItem.ID, fakeItem.TemplateID, fields, this.database, language, itemVersion);

      return item;
    }

    protected FieldList BuildItemFieldList(DbItem fakeItem, ID templateId, Language language, Version version)
    {
      // build a sequence of templates that the item inherits from
      var templates = ExpandTemplatesSequence(templateId);

      var fields = new FieldList();
      foreach (var template in templates)
      {
        AddFieldsFromTemplate(fields, fakeItem, template, language, version);
      }

      // If the item is a Template item we also need to add the BaseTemplate field
      var fakeItemAsTemplate = fakeItem as DbTemplate;
      if (fakeItemAsTemplate != null && fakeItemAsTemplate.BaseIDs != null)
      {
        fields.Add(FieldIDs.BaseTemplate, string.Join("|", fakeItemAsTemplate.BaseIDs.ToList()));
      }

      return fields;
    }

    internal List<DbTemplate> ExpandTemplatesSequence(ID templateId)
    {
      var fakeTemplate = this.GetFakeTemplate(templateId);
      if (fakeTemplate == null)
      {
        return new List<DbTemplate>();
      }

      var sequence = new List<DbTemplate>() {fakeTemplate};

      if (fakeTemplate.BaseIDs != null)
      {
        foreach (var baseId in fakeTemplate.BaseIDs)
        {
          sequence.AddRange(ExpandTemplatesSequence(baseId));
        }
      }

      sequence.Reverse();

      return sequence;
    }

    protected void AddFieldsFromTemplate(FieldList allFields, DbItem fakeItem, DbTemplate fakeTemplate, Language language, Version version)
    {
      var fields = new FieldList(); // this.GetFieldList(fakeTemplate.ID, fakeItem.Name);
      foreach (var templateField in fakeTemplate.Fields)
      {
        var fieldId = templateField.ID;
        var value = string.Empty;

        DbField itemField = FindItemDbField(fakeItem, templateField);

        if (itemField != null)
        {
          value = itemField.GetValue(language.Name, version.Number);
          fields.Add(fieldId, value);
        }
      }

      foreach (KeyValuePair<ID, string> field in fields)
      {
        allFields.Add(field.Key, field.Value);
      }
    }

    protected DbField FindItemDbField(DbItem fakeItem, DbField templateField)
    {
      Assert.IsNotNull(fakeItem, "fakeItem");
      Assert.IsNotNull(templateField, "templateField");

      // The item has fields with the IDs matching the fields in the template it directly inherits from

      if (fakeItem.Fields.InnerFields.ContainsKey(templateField.ID))
      {
        return fakeItem.Fields[templateField.ID];
      }

      return null;
    }

    protected void FillDefaultFakeTemplates()
    {
      this.FakeTemplates.Add(TemplateIdSitecore, new DbTemplate("Sitecore", new TemplateID(TemplateIdSitecore)) { new DbField(FieldIDs.Security) });
      this.FakeTemplates.Add(TemplateIDs.MainSection, new DbTemplate("Main Section", TemplateIDs.MainSection));

      this.FakeTemplates.Add(TemplateIDs.Template, new DbTemplate(TemplateItemName, TemplateIDs.Template) { new DbField(FieldIDs.BaseTemplate) });
      this.FakeTemplates.Add(TemplateIDs.Folder, new DbTemplate(FolderItemName, TemplateIDs.Folder));
    }

    protected void FillDefaultFakeItems()
    {
      var field = new DbField("__Security") { Value = "ar|Everyone|p*|+*|" };

      this.FakeItems.Add(ItemIDs.RootID, new DbItem(SitecoreItemName, ItemIDs.RootID, TemplateIdSitecore) { ParentID = ID.Null, FullPath = "/sitecore", Fields = { field } });
      this.FakeItems.Add(ItemIDs.ContentRoot, new DbItem(ContentItemName, ItemIDs.ContentRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/content" });
      this.FakeItems.Add(ItemIDs.TemplateRoot, new DbItem(TemplatesItemName, ItemIDs.TemplateRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/templates" });
      this.FakeItems.Add(ItemIDs.SystemRoot, new DbItem(SystemItemName, ItemIDs.SystemRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/system" });
      this.FakeItems.Add(ItemIDs.MediaLibraryRoot, new DbItem(MediaLibraryItemName, ItemIDs.MediaLibraryRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/media library" });

      this.FakeItems[ItemIDs.RootID].Add(this.FakeItems[ItemIDs.ContentRoot]);
      this.FakeItems[ItemIDs.RootID].Add(this.FakeItems[ItemIDs.TemplateRoot]);
      this.FakeItems[ItemIDs.RootID].Add(this.FakeItems[ItemIDs.SystemRoot]);
      this.FakeItems[ItemIDs.RootID].Add(this.FakeItems[ItemIDs.MediaLibraryRoot]);

      // TODO: Move 'Template' item to proper directory to correspond Sitecore structure.
      this.FakeItems.Add(TemplateIDs.Template, new DbItem(TemplateItemName, TemplateIDs.Template, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template" });
      this.FakeItems.Add(TemplateIDs.TemplateSection, new DbItem(TemplateSectionItemName, TemplateIDs.TemplateSection, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template section" });
      this.FakeItems.Add(TemplateIDs.TemplateField, new DbItem(TemplateFieldItemName, TemplateIDs.TemplateField, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template field" });
      this.FakeItems.Add(TemplateIDs.BranchTemplate, new DbItem(BranchItemName, TemplateIDs.BranchTemplate, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/branch" });
    }
  }
}