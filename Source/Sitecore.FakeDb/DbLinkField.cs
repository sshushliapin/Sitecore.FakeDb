namespace Sitecore.FakeDb
{
  using System.Xml.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

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

    public string Anchor
    {
      get
      {
        return this.link.ToString();
      }

      set
      {
        Assert.ArgumentNotNull(value, "anchor");
        this.link.SetAttributeValue("anchor", value);
      }
    }

    public string Url
    {
      get
      {
        return this.link.ToString();
      }

      set
      {
        Assert.ArgumentNotNull(value, "url");
        this.link.SetAttributeValue("url", value);
      }
    }

    public string Text
    {
      get
      {
        return this.link.ToString();
      }

      set
      {
        Assert.ArgumentNotNull(value, "text");
        this.link.SetAttributeValue("text", value);
      }
    }

    public string LinkType
    {
      get
      {
        return this.link.ToString();
      }

      set
      {
        Assert.ArgumentNotNull(value, "linktype");
        this.link.SetAttributeValue("linktype", value);
      }
    }

    public string Class
    {
      get
      {
        return this.link.ToString();
      }

      set
      {
        Assert.ArgumentNotNull(value, "class");
        this.link.SetAttributeValue("class", value);
      }
    }

    public string Title
    {
      get
      {
        return this.link.ToString();
      }

      set
      {
        Assert.ArgumentNotNull(value, "title");
        this.link.SetAttributeValue("title", value);
      }
    }

    public string Target
    {
      get
      {
        return this.link.ToString();
      }

      set
      {
        Assert.ArgumentNotNull(value, "target");
        this.link.SetAttributeValue("target", value);
      }
    }

    public string QueryString
    {
      get
      {
        return this.link.ToString();
      }

      set
      {
        Assert.ArgumentNotNull(value, "querystring");
        this.link.SetAttributeValue("querystring", value);
      }
    }

    public ID TargetID
    {
      get
      {
        return ID.Null;
      }

      set
      {
        Assert.ArgumentNotNull(value, "id");
        this.link.SetAttributeValue("id", value);
      }
    }

    public override string GetValue(string language, int version)
    {
      return this.link.ToString();
    }
  }
}