namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Data;

  public class DbFieldCollection : IEnumerable<DbField>
  {
    private readonly IDictionary<ID, DbField> fields = new Dictionary<ID, DbField>();

    // TODO: Get rid of this
    public IDictionary<ID, DbField> InnerFields
    {
      get { return this.fields; }
    }

    public DbField this[ID id]
    {
      get { return this.fields.Values.Single(f => f.ID == id); }
      set { this.fields[value.ID] = value; }
    }

    public void Add(string fieldName)
    {
      this.Add(fieldName, string.Empty);
    }

    public void Add(string fieldName, string fieldValue)
    {
      var field = new DbField(fieldName) { Value = fieldValue };

      this.Add(field);
    }

    public void Add(DbField field)
    {
      if (ID.IsNullOrEmpty(field.ID))
      {
        field.ID = ID.NewID;
      }

      this.fields.Add(field.ID, field);
    }

    public IEnumerator<DbField> GetEnumerator()
    {
      return this.fields.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }
}