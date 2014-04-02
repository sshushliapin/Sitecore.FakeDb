using System.Data.Odbc;

namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class DbFieldCollection : IEnumerable<DbField>
  {
    private readonly IDictionary<ID, DbField> fields = new Dictionary<ID, DbField>();

    // TODO: Get rid of this
    internal IDictionary<ID, DbField> InnerFields
    {
      get { return this.fields; }
    }

    public DbField this[ID id]
    {
      // ToDo: Sitecore Item.Fields[ID] always returns a field instance and then defaults to returning string.Empty for .Value.
      // Only [string name] and [int index] indexers return null when not found. Should we unify?

      get { return this.fields.Values.SingleOrDefault(f => f.ID == id); }
      set { this.fields[value.ID] = value; }
    }

    public void Add(string fieldName)
    {
      this.Add(fieldName, string.Empty);
    }

    public void Add(string fieldName, string fieldValue)
    {
      Assert.ArgumentNotNullOrEmpty(fieldName, "fieldName");

      var field = new DbField(fieldName) { Value = fieldValue };

      this.Add(field);
    }

    public void Add(DbField field)
    {
      Assert.ArgumentNotNull(field, "field");

      if (ID.IsNullOrEmpty(field.ID))
      {
        field.ID = ID.NewID;
      }

      if (this.fields.ContainsKey(field.ID))
      {
        this.fields[field.ID] = field;
      }
      else
      {
        this.fields.Add(field.ID, field);
      }
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