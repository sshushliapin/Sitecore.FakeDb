namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
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

    public DbItem(string name, ID id, ID templateId, params DbItem[] children)
      : this(name, id, templateId)
    {
      if (children != null)
      {
        foreach (var child in children)
        {
          this.Add(child);
        }
      }
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

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.Children.GetEnumerator();
    }
  }
}