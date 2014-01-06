namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using Sitecore.Data;

  public class FItem : IEnumerable
  {
    public FItem(string name)
      : this(name, ID.NewID)
    {
    }

    public FItem(string name, ID id)
    {
      this.Name = name;
      this.ID = id;
      this.TemplateID = ID.NewID;
      this.Fields = new Dictionary<string, object>();
    }

    public string Name { get; private set; }

    public ID ID { get; private set; }

    public ID TemplateID { get; private set; }

    public IDictionary<string, object> Fields { get; private set; }

    public void Add(string fieldName, string fieldValue)
    {
      this.Fields.Add(fieldName, fieldValue);
    }

    public IEnumerator GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}