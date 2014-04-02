using Sitecore.Diagnostics;
using Sitecore.FakeDb.Data.Engines;

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
    private readonly DbFieldCollection _fields = new DbFieldCollection();

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
      this.Children = new Collection<DbItem>();

      // ToDo: standard fields should be coming from the standard template that every item should inherit from unless specified otherwise
      this.Fields.Add(new DbField(FieldIDs.StandardValues));
      this.Fields.Add(new DbField(FieldIDs.LayoutField));
    }

    public string Name { get; set; }

    public ID ID { get; private set; }

    public ID TemplateID { get; set; }

    public DbFieldCollection Fields
    {
      get { return _fields; }
      set
      {
        Assert.ArgumentNotNull(value, "value");

        foreach (var field in value)
        {
          this._fields.Add(field);
        }
      }
    }

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