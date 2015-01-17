namespace Sitecore.FakeDb.Tests
{
  using System.Xml.Linq;
  using Sitecore.Data;

  // <link linktype="external" url="http://gmail.com" anchor="" target="" />
  public class DbLinkField : DbField
  {
    private readonly XElement link = new XElement("link");

    public DbLinkField(ID id)
      : base(id)
    {
    }

    public DbLinkField(string name)
      : base(name)
    {
    }

    public DbLinkField(string name, ID id)
      : base(name, id)
    {
    }

    public string Url
    {
      get
      {
        return this.link.ToString();
      }

      set
      {
        this.link.SetAttributeValue("url", value);
      }
    }

    public override string GetValue(string language, int version)
    {
      return this.link.ToString();
    }
  }
}