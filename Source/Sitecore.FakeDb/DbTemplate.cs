namespace Sitecore.FakeDb
{
  using System.Collections;
  using Sitecore.Data;

  public class DbTemplate : IEnumerable
  {
    public string Name { get; private set; }

    public ID ID { get; private set; }

    public DbFieldCollection Fields { get; set; }

    public DbTemplate()
      : this(null, ID.NewID)
    {
    }

    public DbTemplate(string name, ID id)
    {
      this.Name = name ?? id.ToShortID().ToString();
      this.ID = id;

      this.Fields = new DbFieldCollection();
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