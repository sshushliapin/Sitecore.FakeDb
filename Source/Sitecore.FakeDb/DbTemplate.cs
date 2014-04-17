namespace Sitecore.FakeDb
{
  using System.Collections;
  using Sitecore.Data;

  public class DbTemplate : IEnumerable
  {
    public string Name { get; private set; }

    public ID ID { get; internal set; }

    public DbFieldCollection Fields { get; private set; }

    internal DbFieldCollection StandardValues { get; private set; }

    public DbTemplate()
      : this(null)
    {
    }

    public DbTemplate(string name)
      : this(name, ID.Null)
    {
    }

    public DbTemplate(string name, ID id)
      : this(name, id, new DbFieldCollection())
    {
    }

    public DbTemplate(string name, ID id, DbFieldCollection fields)
    {
      this.Name = name;
      this.ID = id;

      this.Fields = fields;
      this.StandardValues = new DbFieldCollection();
    }

    public void Add(string fieldName)
    {
      this.Add(fieldName, string.Empty);
    }

    public void Add(string fieldName, string standardValue)
    {
      var id = ID.NewID;
      var field = new DbField(fieldName, id);

      this.Add(field, standardValue);
    }

    public void Add(ID id)
    {
      this.Add(id, string.Empty);
    }

    public void Add(ID id, string standardValue)
    {
      var fieldName = id.ToShortID().ToString();
      var field = new DbField(fieldName, id);

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