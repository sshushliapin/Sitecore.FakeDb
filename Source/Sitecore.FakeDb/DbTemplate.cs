namespace Sitecore.FakeDb
{
  using System.Collections;
  using Sitecore.Data;

  public class DbTemplate : IEnumerable
  {
    public string Name { get; private set; }

    public ID ID { get; set; }

    public DbFieldCollection Fields { get; set; }

    public DbTemplate()
      : this(null)
    {
    }

    public DbTemplate(string name)
    {
      this.Name = name;
      this.Fields = new DbFieldCollection();
    }

    public DbTemplate(string name, ID id)
      : this(name)
    {
      this.ID = id;
    }

    public void Add(string fieldName)
    {
      this.Fields.Add(fieldName);
    }

    public IEnumerator GetEnumerator()
    {
      return this.Fields.GetEnumerator();
    }
  }
}