namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Security.AccessControl;

  /// <summary>
  /// Represents a lightweight version of the <see cref="Item"/> class.
  /// </summary>
  [DebuggerDisplay("{FullPath}, {ID.ToString()}")]
  public class DbItem : IEnumerable
  {
    private readonly IDictionary<string, int> versionsCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbItem"/> class.
    /// </summary>
    /// <param name="name">The item name.</param>
    public DbItem(string name)
      : this(name, ID.NewID)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbItem"/> class.
    /// </summary>
    /// <param name="name">The item name.</param>
    /// <param name="id">The item id.</param>
    public DbItem(string name, ID id)
      : this(name, id, ID.Null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbItem"/> class.
    /// </summary>
    /// <param name="name">The item name.</param>
    /// <param name="id">The item id.</param>
    /// <param name="templateId">The template id.</param>
    public DbItem(string name, ID id, ID templateId)
    {
      this.Name = !string.IsNullOrEmpty(name) ? name : id.ToShortID().ToString();
      this.ID = id;
      this.TemplateID = templateId;
      this.Access = new DbItemAccess();
      this.Fields = new DbFieldCollection();
      this.Children = new DbItemChildCollection(this);
      this.versionsCount = new Dictionary<string, int>();
    }

    public string Name { get; set; }

    public ID ID { get; private set; }

    public ID TemplateID { get; set; }

    public ID BranchId { get; set; }

    public DbFieldCollection Fields { get; private set; }

    public ID ParentID { get; set; }

    public string FullPath { get; set; }

    public ICollection<DbItem> Children { get; private set; }

    public DbItemAccess Access { get; set; }

    /// <summary>
    /// Similar to Item.Database to know at runtime what Database instance a DbItem belongs to
    /// </summary>
    internal Database Database { get; set; }

    /// <summary>
    /// Adds a new field to the item.
    /// </summary>
    /// <param name="fieldName">The field name.</param>
    /// <param name="fieldValue">The field value.</param>
    public void Add(string fieldName, string fieldValue)
    {
      Assert.ArgumentNotNull(fieldName, "fieldName");

      this.Fields.Add(fieldName, fieldValue);
    }

    /// <summary>
    /// Adds a new field to the item.
    /// </summary>
    /// <param name="fieldId">The field id.</param>
    /// <param name="fieldValue">The field value.</param>
    public void Add(ID fieldId, string fieldValue)
    {
      Assert.ArgumentNotNull(fieldId, "fieldId");

      this.Fields.Add(fieldId, fieldValue);
    }

    /// <summary>
    /// Adds a new <see cref="DbField"/> to the item.
    /// </summary>
    /// <param name="field">The field.</param>
    public void Add(DbField field)
    {
      Assert.ArgumentNotNull(field, "field");

      this.Fields.Add(field);
    }

    /// <summary>
    /// Adds a child <see cref="DbItem"/> to the item.
    /// </summary>
    /// <param name="child">The child item.</param>
    public void Add(DbItem child)
    {
      Assert.ArgumentNotNull(child, "child");

      this.Children.Add(child);
    }

    /// <summary>
    /// Adds a new version to the item in specific language.
    /// </summary>
    /// <param name="language">The language.</param>
    public void AddVersion(string language)
    {
      this.AddVersion(language, 0);
    }

    /// <summary>
    /// Adds a new version to the item in specific language.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <param name="currentVersion">The current varsion.</param>
    public void AddVersion(string language, int currentVersion)
    {
      Assert.ArgumentNotNull(language, "language");
      if (currentVersion < 0)
      {
        throw new ArgumentOutOfRangeException("currentVersion");
      }

      if (currentVersion == 0)
      {
        if (this.versionsCount.ContainsKey(language))
        {
          this.versionsCount[language] += 1;
        }
        else
        {
          this.versionsCount[language] = 1;
        }
        return;
      }

      foreach (var field in this.Fields)
      {
        var value = field.GetValue(language, currentVersion);
        field.Add(language, value);
      }

      this.versionsCount[language] = ++currentVersion;
    }

    /// <summary>
    /// Gets the item versions count.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <returns>The version count.</returns>
    public int GetVersionCount(string language)
    {
      Assert.ArgumentNotNull(language, "language");

      var versionsCount = 0;

      if (this.versionsCount.ContainsKey(language))
      {
        versionsCount = this.versionsCount[language];
      }

      foreach (var field in this.Fields)
      {
        if (!field.Values.ContainsKey(language))
        {
          continue;
        }

        var maxVersion = field.Values[language].Keys.OrderBy(k => k).LastOrDefault();
        if (maxVersion > versionsCount)
        {
          versionsCount = maxVersion;
        }
      }

      return versionsCount;
    }

    /// <summary>
    /// Removes the item version.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <returns>true if varsion was successfully removed; otherwise, false.</returns>
    public bool RemoveVersion(string language)
    {
      Assert.ArgumentNotNull(language, "language");

      var removed = false;

      foreach (var field in this.Fields)
      {
        if (!field.Values.ContainsKey(language))
        {
          continue;
        }

        var langValues = field.Values[language];
        var lastVersion = langValues.Last();

        removed = langValues.Remove(lastVersion);
      }

      if (!this.versionsCount.ContainsKey(language))
      {
        return removed;
      }

      this.versionsCount[language] -= 1;

      return true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.Children.GetEnumerator();
    }
  }
}