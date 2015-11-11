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
  using ItemIDs = Sitecore.ItemIDs;
  using Version = Sitecore.Data.Version;

  public class DataStorage
  {
    private static readonly ID TemplateIdSitecore = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private readonly Database database;

    private readonly IDictionary<ID, DbItem> fakeItems;

    private readonly IDictionary<Guid, Stream> blobs;

    public DataStorage(Database database)
    {
      this.database = database;

      this.fakeItems = new Dictionary<ID, DbItem>();
      this.blobs = new Dictionary<Guid, Stream>();

      this.FillDefaultFakeTemplates();
      this.FillDefaultFakeItems();
    }

    public Database Database
    {
      get { return this.database; }
    }

    protected IDictionary<ID, DbItem> FakeItems
    {
      get { return this.fakeItems; }
    }

    protected IDictionary<Guid, Stream> Blobs
    {
      get { return this.blobs; }
    }

    public virtual void AddFakeItem(DbItem item)
    {
      Assert.ArgumentNotNull(item, "item");

      var loading = item is IDsDbItem;
      if (item as DbTemplate != null)
      {
        var template = (DbTemplate)item;

        if (!loading)
        {
          this.AssertDoesNotExists(template);
        }

        if (template is IDsDbItem)
        {
          CorePipeline.Run("loadDsDbTemplate", new DsItemLoadingArgs(template as IDsDbItem, this));
        }
      }

      if (loading)
      {
        CorePipeline.Run("loadDsDbItem", new DsItemLoadingArgs(item as IDsDbItem, this));
      }

      CorePipeline.Run("addDbItem", new AddDbItemArgs(item, this));

      if (!loading)
      {
        this.AssertDoesNotExists(item);
      }

      this.FakeItems[item.ID] = item;

      if (item as DbTemplate != null)
      {
        this.Database.Engines.TemplateEngine.Reset();
      }

      foreach (var child in item.Children)
      {
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
      return this.FakeItems.ContainsKey(templateId) ? this.FakeItems[templateId] as DbTemplate : null;
    }

    public virtual IEnumerable<DbTemplate> GetFakeTemplates()
    {
      return this.FakeItems.Values.OfType<DbTemplate>();
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

    public virtual IEnumerable<DbItem> GetFakeItems()
    {
      return this.FakeItems.Values;
    }

    public virtual bool RemoveFakeItem(ID itemId)
    {
      return this.FakeItems.Remove(itemId);
    }

    public virtual void SetBlobStream(Guid blobId, Stream stream)
    {
      Assert.ArgumentNotNull(stream, "stream");

      this.Blobs[blobId] = stream;
    }

    public virtual Stream GetBlobStream(Guid blobId)
    {
      return this.Blobs.ContainsKey(blobId) ? this.Blobs[blobId] : null;
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
      foreach (var templateField in fakeTemplate.Fields)
      {
        var fieldId = templateField.ID;
        var itemField = this.FindItemDbField(fakeItem, templateField);

        if (itemField == null)
        {
          continue;
        }

        var value = itemField.GetValue(language.Name, version.Number) ?? string.Empty;
        allFields.Add(fieldId, value);
      }

      foreach (var template in fakeTemplate.BaseIDs.Select(this.GetFakeTemplate).Where(t => t != null))
      {
        this.AddFieldsFromTemplate(allFields, fakeItem, template, language, version);
      }

      if (fakeTemplate.BaseIDs.Any() || fakeTemplate.ID == TemplateIDs.StandardTemplate)
      {
        return;
      }

      var standardTemplate = this.GetFakeTemplate(TemplateIDs.StandardTemplate);
      this.AddFieldsFromTemplate(allFields, fakeItem, standardTemplate, language, version);
    }

    protected DbField FindItemDbField(DbItem fakeItem, DbField templateField)
    {
      Assert.IsNotNull(fakeItem, "fakeItem");
      Assert.IsNotNull(templateField, "templateField");

      // The item has fields with the IDs matching the fields in the template it directly inherits from
      if (fakeItem.Fields.ContainsKey(templateField.ID))
      {
        return fakeItem.Fields[templateField.ID];
      }

      return fakeItem.Fields.SingleOrDefault(f => string.Equals(f.Name, templateField.Name));
    }

    protected void FillDefaultFakeTemplates()
    {
      this.FakeItems.Add(TemplateIdSitecore, new DbTemplate("Sitecore", new TemplateID(TemplateIdSitecore)) { new DbField(FieldIDs.Security) });
      this.FakeItems.Add(TemplateIDs.MainSection, new DbTemplate("Main Section", TemplateIDs.MainSection));

      this.FakeItems.Add(
        TemplateIDs.Template,
        new DbTemplate(ItemNames.Template, TemplateIDs.Template)
          {
            ParentID = ItemIDs.TemplateRoot,
            FullPath = "/sitecore/templates/template",
            Fields = { new DbField(FieldIDs.BaseTemplate) }
          });

      this.FakeItems.Add(TemplateIDs.Folder, new DbTemplate(ItemNames.Folder, TemplateIDs.Folder));

      this.FakeItems.Add(
        TemplateIDs.StandardTemplate,
        new DbTemplate(TemplateIDs.StandardTemplate)
          {
            new DbField(FieldIDs.BaseTemplate) { Shared = true },
            new DbField(FieldIDs.Lock) { Shared = true },
            new DbField(FieldIDs.Security) { Shared = true },
            new DbField(FieldIDs.Created),
            new DbField(FieldIDs.CreatedBy),
            new DbField(FieldIDs.Updated),
            new DbField(FieldIDs.UpdatedBy),
            new DbField(FieldIDs.Revision),
            new DbField(FieldIDs.LayoutField) { Type = "Layout" },
            new DbField(DbFieldIds.FinalLayoutField),
            new DbField(FieldIDs.DisplayName),
            new DbField(FieldIDs.Hidden),
            new DbField(FieldIDs.ReadOnly),
            new DbField(FieldIDs.Source),
            new DbField(DbFieldIds.AnalyticsIds.PageLevelTestDefinitionField) { Shared = true },
            new DbField(DbFieldIds.AnalyticsIds.TrackingField) { Type = "Tracking", Shared = true }
          });

      this.FakeItems.Add(
        TemplateIDs.TemplateField,
        new DbTemplate(ItemNames.TemplateField, TemplateIDs.TemplateField, TemplateIDs.TemplateField)
          {
            ParentID = ItemIDs.TemplateRoot,
            FullPath = "/sitecore/templates/template field",
            Fields =
              {
                new DbField(TemplateFieldIDs.Type),
                new DbField(TemplateFieldIDs.Shared),
                new DbField(TemplateFieldIDs.Source)
              }
          });
    }

    protected void FillDefaultFakeItems()
    {
      var field = new DbField("__Security") { Value = "ar|Everyone|p*|+*|" };

      this.FakeItems.Add(ItemIDs.RootID, new DbItem(ItemNames.Sitecore, ItemIDs.RootID, TemplateIdSitecore) { ParentID = ID.Null, FullPath = "/sitecore", Fields = { field } });
      this.FakeItems.Add(ItemIDs.ContentRoot, new DbItem(ItemNames.Content, ItemIDs.ContentRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/content" });
      this.FakeItems.Add(ItemIDs.TemplateRoot, new DbItem(ItemNames.Templates, ItemIDs.TemplateRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/templates" });
      this.FakeItems.Add(ItemIDs.BranchesRoot, new DbItem(ItemNames.Branches, ItemIDs.BranchesRoot, TemplateIDs.BranchTemplateFolder) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/Branches" });
      this.FakeItems.Add(ItemIDs.SystemRoot, new DbItem(ItemNames.System, ItemIDs.SystemRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/system" });
      this.FakeItems.Add(ItemIDs.MediaLibraryRoot, new DbItem(ItemNames.MediaLibrary, ItemIDs.MediaLibraryRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/media library" });

      this.FakeItems[ItemIDs.RootID].Add(this.FakeItems[ItemIDs.ContentRoot]);
      this.FakeItems[ItemIDs.RootID].Add(this.FakeItems[ItemIDs.TemplateRoot]);
      this.FakeItems[ItemIDs.RootID].Add(this.FakeItems[ItemIDs.SystemRoot]);
      this.FakeItems[ItemIDs.RootID].Add(this.FakeItems[ItemIDs.MediaLibraryRoot]);

      // TODO: Move 'Template' item to proper directory to correspond Sitecore structure.
      this.FakeItems.Add(TemplateIDs.TemplateSection, new DbTemplate(ItemNames.TemplateSection, TemplateIDs.TemplateSection, TemplateIDs.TemplateSection) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template section" });
      this.FakeItems.Add(TemplateIDs.BranchTemplate, new DbItem(ItemNames.Branch, TemplateIDs.BranchTemplate, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/branch" });

      this.AddFakeItem(new DbItem(ItemNames.DefinitionsRoot, ItemIDs.Analytics.MarketingCenterItem, TemplateIDs.Folder) { ParentID = ItemIDs.SystemRoot, FullPath = "/sitecore/system/Marketing Control Panel" });
      this.AddFakeItem(new DbItem(ItemNames.Profiles, ItemIDs.Analytics.Profiles, TemplateIDs.Folder) { ParentID = ItemIDs.Analytics.MarketingCenterItem, FullPath = "/sitecore/system/Marketing Control Panel/Profiles" });

      if (this.Database.Name == "core")
      {
        this.AddFakeItem(
          new DbItem(ItemNames.FieldTypes, new ID("{76E6D8C7-1F93-4712-872B-DA3C96B808F2}"), TemplateIDs.Node)
            {
              ParentID = ItemIDs.SystemRoot,
              Children = { new DbItem("text") { { "Control", "Text" } } }
            });
      }
    }

    private void AssertDoesNotExists(DbItem item)
    {
      if (!this.FakeItems.ContainsKey(item.ID))
      {
        return;
      }

      var existingItem = this.FakeItems[item.ID];
      string message;

      if (existingItem is DbTemplate)
      {
        message = string.Format("A template with the same id has already been added ('{0}', '{1}').", item.ID, item.FullPath ?? item.Name);
      }
      else if (existingItem.GetType() == typeof(DbItem) && item is DbTemplate)
      {
        message = string.Format("Unable to create the item based on the template '{0}'. An item with the same id has already been added ('{1}').", item.ID, existingItem.FullPath);
      }
      else
      {
        message = string.Format("An item with the same id has already been added ('{0}', '{1}').", item.ID, item.FullPath);
      }

      throw new InvalidOperationException(message);
    }
  }
}