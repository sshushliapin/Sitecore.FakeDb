namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics;
  using Sitecore.Data;

  [DebuggerDisplay("Name = {Name}, FullPath = {FullPath}")]
  public class DbItem : IEnumerable
  {
    public DbItem(string name)
      : this(name, ID.NewID)
    {
    }

    public DbItem(string name, ID id)
      : this(name, id, ID.NewID)
    {
    }

    public DbItem(string name, ID id, ID templateId)
    {
      this.Name = name;
      this.ID = id;
      this.TemplateID = templateId;
      this.Fields = new Dictionary<string, object>();
      this.Children = new Collection<DbItem>();
    }

    public string Name { get; set; }

    public ID ID { get; private set; }

    public ID TemplateID { get; private set; }

    public IDictionary<string, object> Fields { get; private set; }

    public ID ParentID { get; set; }

    public string FullPath { get; internal set; }

    public ICollection<DbItem> Children { get; private set; }

    public void Add(string fieldName)
    {
      this.Fields.Add(fieldName, string.Empty);
    }

    public void Add(string fieldName, string fieldValue)
    {
      this.Fields.Add(fieldName, fieldValue);
    }

    public void Add(DbItem child)
    {
      child.ParentID = this.ID;

      this.Children.Add(child);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.Children.GetEnumerator();
    }
  }
}