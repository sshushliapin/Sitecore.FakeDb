namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Security.AccessControl;

  [DebuggerDisplay("{FullPath}, {ID.ToString()}")]
  public class DbItem : IEnumerable
  {
    public DbItem(string name)
      : this(name, ID.NewID)
    {
    }

    public DbItem(string name, ID id)
      : this(name, id, ID.Null)
    {
    }

    public DbItem(string name, ID id, ID templateId)
    {
      this.Name = !string.IsNullOrEmpty(name) ? name : id.ToShortID().ToString();
      this.ID = id;
      this.TemplateID = templateId;
      this.Access = new DbItemAccess();
      this.Fields = new DbFieldCollection();
      this.Children = new DbItemChildCollection(this);
      this.VersionsCount = new Dictionary<string, int>();
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

    public IDictionary<string, int> VersionsCount { get; private set; }

    public void Add(string fieldName, string fieldValue)
    {
      Assert.ArgumentNotNull(fieldName, "fieldName");

      this.Fields.Add(fieldName, fieldValue);
    }

    public void Add(ID fieldId, string fieldValue)
    {
      Assert.ArgumentNotNull(fieldId, "fieldId");

      this.Fields.Add(fieldId, fieldValue);
    }

    public void Add(DbField field)
    {
      Assert.ArgumentNotNull(field, "field");

      this.Fields.Add(field);
    }

    public void Add(DbItem child)
    {
      Assert.ArgumentNotNull(child, "child");

      this.Children.Add(child);
    }

    public void AddVersion(string language)
    {
      this.AddVersion(language, 0);
    }

    public void AddVersion(string language, int currentVersion)
    {
      Assert.ArgumentNotNull(language, "language");
      if (currentVersion < 0)
      {
        throw new ArgumentOutOfRangeException("currentVersion");
      }

      if (currentVersion == 0)
      {
        this.VersionsCount[language] = 1;
        return;
      }

      foreach (var field in this.Fields)
      {
        var value = field.GetValue(language, currentVersion);
        field.Add(language, value);
      }

      this.VersionsCount[language] = ++currentVersion;
    }

    public int GetVersionCount(string language)
    {
      Assert.ArgumentNotNull(language, "language");

      var versionsCount = 0;

      if (this.VersionsCount.ContainsKey(language))
      {
        versionsCount = this.VersionsCount[language];
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

      if (!this.VersionsCount.ContainsKey(language))
      {
        return removed;
      }

      this.VersionsCount[language] -= 1;

      return true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.Children.GetEnumerator();
    }
  }
}