namespace Sitecore.FakeDb.Templates
{
  using System.Collections;
  using System.Collections.Generic;
  using Sitecore.Data;

  public class FTemplate : IEnumerable
  {
    public string Name { get; private set; }

    public ID ID { get; private set; }

    public ICollection<string> Fields { get; set; }

    public FTemplate()
      : this(null, ID.NewID)
    {
    }

    public FTemplate(string name, ID id)
    {
      this.Name = name ?? id.ToShortID().ToString(); ;
      this.ID = id;

      this.Fields = new List<string>();
    }

    public void Add(string fieldName)
    {
      this.Fields.Add(fieldName);
    }

    public IEnumerator GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}