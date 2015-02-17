namespace Sitecore.FakeDb
{
  using System.Collections;
  using Sitecore.Data;

  public class DbTemplate : DbItem
  {
    public ID[] BaseIDs { get; set; }

    internal DbFieldCollection StandardValues { get; private set; }

    public DbTemplate()
      : this((string)null)
    {
    }

    public DbTemplate(ID id)
      : this(null, id)
    {
    }

    public DbTemplate(string name)
      : this(name, ID.NewID)
    {
    }

    public DbTemplate(string name, ID id)
      : base(name, ID.IsNullOrEmpty(id) ? ID.NewID : id, TemplateIDs.Template)
    {
      this.StandardValues = new DbFieldCollection();

      this.Add(new DbField(FieldIDs.BaseTemplate) { Shared = true });

      // ToDo: we can move these out into the standard template. we have tempalte inheritance now
      this.Add(new DbField(FieldIDs.Lock) { Shared = true });
      this.Add(new DbField(FieldIDs.Security) { Shared = true });
      this.Add(new DbField(FieldIDs.Created));
      this.Add(new DbField(FieldIDs.CreatedBy));
      this.Add(new DbField(FieldIDs.Updated));
      this.Add(new DbField(FieldIDs.UpdatedBy));
      this.Add(new DbField(FieldIDs.Revision));

      this.Add(new DbField(FieldIDs.Hidden));
    }

    public void Add(string fieldName)
    {
      this.Add(fieldName, string.Empty);
    }

    public void Add(ID id)
    {
      this.Add(id, string.Empty);
    }

    public new void Add(string fieldName, string standardValue)
    {
      var field = new DbField(fieldName);

      this.Add(field, standardValue);
    }

    public void Add(ID id, string standardValue)
    {
      var field = new DbField(id);

      this.Add(field, standardValue);
    }

    protected void Add(DbField field, string standardValue)
    {
      this.Fields.Add(field);

      var standardValueField = new DbField(field.Name, field.ID) { Value = standardValue };
      this.StandardValues.Add(standardValueField);
    }

    public IEnumerator GetEnumerator()
    {
      return this.Fields.GetEnumerator();
    }
  }
}