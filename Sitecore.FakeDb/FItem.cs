namespace Sitecore.FakeDb
{
  using Sitecore.Data;

  public class FItem
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
    }

    public string Name { get; private set; }

    public ID ID { get; private set; }

    public ID TemplateID { get; private set; }
  }
}