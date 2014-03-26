using Sitecore.Diagnostics;

namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics;
  using Sitecore.Data;
  using Sitecore.FakeDb.Security.AccessControl;

  [DebuggerDisplay("Name = {Name}, FullPath = {FullPath}")]
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
      this.Name = name;
      this.ID = id;
      this.TemplateID = templateId;
      this.Access = new DbItemAccess();
      this.Fields = new DbFieldCollection();
      this.StandardFields = new DbFieldCollection();
      this.Children = new Collection<DbItem>();
    }
    
    public string Name { get; set; }

    public ID ID { get; private set; }

    public ID TemplateID { get; set; }

    public DbFieldCollection Fields { get; set; }

    public DbFieldCollection StandardFields { get; private set; }

    public ID ParentID { get; set; }

    public string FullPath { get; set; }

    public ICollection<DbItem> Children { get; set; }

    public DbItemAccess Access { get; set; }

    public virtual void Add(string fieldName, string fieldValue)
    {
      this.Fields.Add(fieldName, fieldValue);
    }

    public virtual void Add(DbField field)
    {
      Assert.ArgumentNotNull(field, "Field");
      
      this.Fields.Add(field);
    }

    public virtual void Add(DbItem child)
    {
      Assert.ArgumentNotNull(child, "Child");
      
      this.Children.Add(child);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.Children.GetEnumerator();
    }
  }
}