namespace Sitecore.FakeDb.Data.Engines
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Pipelines;
  using Sitecore.FakeDb.Pipelines.AddDbItem;
  using Sitecore.Globalization;
  using Sitecore.Pipelines;
  using Version = Sitecore.Data.Version;

  public class DataStorage
  {
    private const string SitecoreItemName = "sitecore";

    private const string ContentItemName = "content";

    private const string TemplatesItemName = "templates";

    private const string BranchesItemName = "Branches";

    private const string SystemItemName = "system";

    private const string MediaLibraryItemName = "media library";

    private const string TemplateItemName = "Template";

    private const string TemplateSectionItemName = "Template section";

    private const string TemplateFieldItemName = "Template field";

    private const string BranchItemName = "Branch";

    private const string FolderItemName = "Folder";

    private static readonly ID TemplateIdSitecore = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private readonly Database database;

    private readonly IDictionary<ID, DbItem> fakeItems;

    private readonly IDictionary<ID, DbTemplate> fakeTemplates;

    private readonly IDictionary<Guid, Stream> blobs;

    public DataStorage(Database database)
      : this()
    {
      this.database = database;

      this.FillDefaultFakeTemplates();
      this.FillDefaultFakeItems();
    }

    internal DataStorage()
    {
      this.fakeItems = new Dictionary<ID, DbItem>();
      this.fakeTemplates = new Dictionary<ID, DbTemplate>();
      this.blobs = new Dictionary<Guid, Stream>();
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

    public IDictionary<Guid, Stream> Blobs
    {
      get { return this.blobs; }
    }

    public virtual void AddFakeItem(DbItem item)
    {
      Assert.ArgumentNotNull(item, "item");

      if (item as DbTemplate != null)
      {
        var template = (DbTemplate)item;
        Assert.ArgumentCondition(!this.FakeTemplates.ContainsKey(template.ID), "template", "A template with the same id has already been added.");

        if (template is IDsDbItem)
        {
          CorePipeline.Run("loadDsDbTemplate", new DsItemLoadingArgs(template as IDsDbItem, this));
        }

        this.FakeTemplates.Add(template.ID, template);
        this.Database.Engines.TemplateEngine.Reset();
      }

      if (item is IDsDbItem)
      {
        CorePipeline.Run("loadDsDbItem", new DsItemLoadingArgs(item as IDsDbItem, this));
      }

      CorePipeline.Run("addDbItem", new AddDbItemArgs(item, this));

      this.FakeItems.Add(item.ID, item);
      foreach (var child in item.Children)
      {
        child.ParentID = item.ID;
        child.FullPath = item.FullPath + "/" + child.Name;
        this.AddFakeItem(child);
      }
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

    public virtual Item GetSitecoreItem(ID itemId)
    {
      return this.GetSitecoreItem(itemId, Language.Current);
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

      var fields = this.BuildItemFieldList(fakeItem, fakeItem.TemplateID, language, itemVersion);

      return ItemHelper.CreateInstance(this.database, fakeItem.Name, fakeItem.ID, fakeItem.TemplateID, fakeItem.BranchId, fields, language, itemVersion);
    }
    
    protected FieldList BuildItemFieldList(DbItem fakeItem, ID templateId, Language language, Version version)
    {
      // build a sequence of templates that the item inherits from
      var templates = this.ExpandTemplatesSequence(templateId);

      var fields = new FieldList();
      foreach (var template in templates)
      {
        this.AddFieldsFromTemplate(fields, fakeItem, template, language, version);
      }

      // If the item is a Template item we also need to add the BaseTemplate field
      var fakeItemAsTemplate = fakeItem as DbTemplate;
      if (fakeItemAsTemplate != null && fakeItemAsTemplate.BaseIDs != null)
      {
        fields.Add(FieldIDs.BaseTemplate, string.Join("|", fakeItemAsTemplate.BaseIDs.ToList()));
      }

      return fields;
    }

    /// <summary>
    /// Similar to Template.GetBaseTemplates() the method expands the template inheritance hierarchy
    /// </summary>
    /// <param name="templateId">The template id.</param>
    /// <returns>The list of tempaltes.</returns>
    protected List<DbTemplate> ExpandTemplatesSequence(ID templateId)
    {
      var fakeTemplate = this.GetFakeTemplate(templateId);
      if (fakeTemplate == null)
      {
        return new List<DbTemplate>();
      }

      var sequence = new List<DbTemplate> { fakeTemplate };

      if (fakeTemplate.BaseIDs != null)
      {
        foreach (var baseId in fakeTemplate.BaseIDs)
        {
          sequence.AddRange(this.ExpandTemplatesSequence(baseId));
        }
      }

      sequence.Reverse();

      return sequence;
    }

    protected void AddFieldsFromTemplate(FieldList allFields, DbItem fakeItem, DbTemplate fakeTemplate, Language language, Version version)
    {
      var fields = new FieldList();
      foreach (var templateField in fakeTemplate.Fields)
      {
        var fieldId = templateField.ID;

        var itemField = this.FindItemDbField(fakeItem, templateField);

        if (itemField == null)
        {
          continue;
        }

        var value = itemField.GetValue(language.Name, version.Number);
        fields.Add(fieldId, value);
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

      return fakeItem.Fields.InnerFields.Values.SingleOrDefault(f => string.Equals(f.Name, templateField.Name));
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
      this.FakeItems.Add(ItemIDs.BranchesRoot, new DbItem(BranchesItemName, ItemIDs.BranchesRoot, TemplateIDs.BranchTemplateFolder) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/Branches" });
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