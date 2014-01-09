namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using Sitecore.Data;

  [DebuggerDisplay("Name = {Name}, ID = {ID}, FullPath = {FullPath}")]
  public class FItem : IEnumerable
  {
    public FItem(string name)
      : this(name, ID.NewID)
    {
    }

    public FItem(string name, ID id)
      : this(name, id, ID.NewID)
    {
    }

    public FItem(string name, ID id, ID templateId)
    {
      this.Name = name;
      this.ID = id;
      this.TemplateID = templateId;
      this.Fields = new Dictionary<string, object>();
      this.ParentID = ItemIDs.ContentRoot;
      this.FullPath = Constants.ContentPath + "/" + name;
    }

    public string Name { get; private set; }

    public ID ID { get; private set; }

    public ID TemplateID { get; private set; }

    public IDictionary<string, object> Fields { get; private set; }

    public ID ParentID { get; set; }

    public string FullPath { get; set; }

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