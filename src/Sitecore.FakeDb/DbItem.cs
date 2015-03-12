namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics;
  using Sitecore.Data;
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
      this.Children = new Collection<DbItem>();
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
      this.Fields.Add(fieldName, fieldValue);
    }

    public void Add(ID fieldId, string fieldValue)
    {
      this.Fields.Add(fieldId, fieldValue);
    }

    public void Add(DbField field)
    {
      this.Fields.Add(field);
    }

    public void Add(DbItem child)
    {
      this.Children.Add(child);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.Children.GetEnumerator();
    }
  }
}