namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using Sitecore.Data;

  public class DbTemplate : IEnumerable
  {
    public string Name { get; private set; }

    public ID ID { get; private set; }

    // TODO: Key is a field name, value is a field id. It's hard to grasp.
    public IDictionary<string, ID> Fields { get; set; }

    public DbTemplate()
      : this(null, ID.NewID)
    {
    }

    public DbTemplate(string name, ID id)
    {
      this.Name = name ?? id.ToShortID().ToString();
      this.ID = id;

      this.Fields = new Dictionary<string, ID>();
    }

    public void Add(string fieldName)
    {
      this.Fields.Add(fieldName, ID.NewID);
    }

    public IEnumerator GetEnumerator()
    {
      return this.Fields.GetEnumerator();
    }
  }
}