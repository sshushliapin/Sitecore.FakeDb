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
    private static readonly ID RootTemplateId = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private static readonly ID TemplateIdSitecore = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private const string SitecoreItemName = "sitecore";

    private const string ContentItemName = "content";

    private const string TemplatesItemName = "templates";

    private const string SystemItemName = "system";

    private const string MediaLibraryItemName = "media library";

    public const string TemplateItemName = "Template";

    public const string TemplateSectionItemName = "Template section";

    public const string TemplateFieldItemName = "Template field";

    public const string BranchItemName = "Branch";

    public const string FolderItemName = "Folder";

    private readonly Database database;

    public DataStorage(Database database)
    {
      this.database = database;

      this.FakeItems = new Dictionary<ID, DbItem>();
      this.FakeTemplates = new Dictionary<ID, DbTemplate>();

      this.FillDefaultFakeTemplates();
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
      return GetFieldList(templateId, null);
    }

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
      if (!this.FakeItems.ContainsKey(itemId))
      {
        return null;
      }

      // TODO:[High] Avoid the templates resetting. Required to avoid sharing templates between unit tests.
      this.Database.Engines.TemplateEngine.Reset();

      var fakeItem = this.FakeItems[itemId];
      var fakeTemplate = this.GetFakeTemplate(fakeItem.TemplateID);
      var itemVersion = version == Version.Latest ? Version.First : version;

      var fields = this.GetFieldList(fakeItem.TemplateID, fakeItem.Name);
      foreach (var templateField in fakeTemplate.Fields)
      {
        var fieldId = templateField.ID;
        var value = string.Empty;

        if (fakeItem.Fields.InnerFields.ContainsKey(fieldId))
        {
          var itemField = fakeItem.Fields[fieldId];

          value = itemField.GetValue(language.Name, version.Number);
        }

        if (string.IsNullOrEmpty(value) && fakeTemplate.StandardValues.InnerFields.ContainsKey(fieldId))
        {
          value = fakeTemplate.StandardValues[fieldId].Value;
          if (value == "$name")
          {
            value = fakeItem.Name;
          }
        }

        fields.Add(fieldId, value);
      }

      var fakeItemAsTemplate = fakeItem as DbTemplate;
      if (fakeItemAsTemplate != null && fakeItemAsTemplate.BaseIDs != null)
      {
        fields.Add(FieldIDs.BaseTemplate, string.Join("|", fakeItemAsTemplate.BaseIDs.ToList()));
      }

      var item = ItemHelper.CreateInstance(fakeItem.Name, fakeItem.ID, fakeItem.TemplateID, fields, this.database, language, itemVersion);

      return item;
    }

    protected virtual void FillDefaultFakeTemplates()
    {
      this.FakeTemplates.Add(TemplateIdSitecore, new DbTemplate("Sitecore", new TemplateID(TemplateIdSitecore)));
      this.FakeTemplates.Add(TemplateIDs.MainSection, new DbTemplate("Main Section", TemplateIDs.MainSection));

      this.FakeTemplates.Add(TemplateIDs.Template, new DbTemplate(TemplateItemName, TemplateIDs.Template) { DbField.FieldIdToNameMapping[FieldIDs.BaseTemplate] });
      this.FakeTemplates.Add(TemplateIDs.Folder, new DbTemplate(FolderItemName, TemplateIDs.Folder));
    }

    protected virtual void FillDefaultFakeItems()
    {
      this.FakeItems.Add(ItemIDs.RootID, new DbItem(SitecoreItemName, ItemIDs.RootID, RootTemplateId) { ParentID = ID.Null, FullPath = "/sitecore" });
      this.FakeItems.Add(ItemIDs.ContentRoot, new DbItem(ContentItemName, ItemIDs.ContentRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/content" });
      this.FakeItems.Add(ItemIDs.TemplateRoot, new DbItem(TemplatesItemName, ItemIDs.TemplateRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/templates" });
      this.FakeItems.Add(ItemIDs.SystemRoot, new DbItem(SystemItemName, ItemIDs.SystemRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/system"});
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