namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class DbLinkField : DbField
  {
    private readonly IDictionary<string, string> attributes = new SortedDictionary<string, string>();

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
        return this.GetAttribute("anchor");
      }

      set
      {
        Assert.ArgumentNotNull(value, "anchor");
        this.attributes["anchor"] = value;
      }
    }

    public string Url
    {
      get
      {
        return this.GetAttribute("url");
      }

      set
      {
        Assert.ArgumentNotNull(value, "url");
        this.attributes["url"] = value;
      }
    }

    public string Text
    {
      get
      {
        return this.GetAttribute("text");
      }

      set
      {
        Assert.ArgumentNotNull(value, "text");
        this.attributes["text"] = value;
      }
    }

    public string LinkType
    {
      get
      {
        return this.GetAttribute("linktype");
      }

      set
      {
        Assert.ArgumentNotNull(value, "linktype");
        this.attributes["linktype"] = value;
      }
    }

    public string Class
    {
      get
      {
        return this.GetAttribute("class");
      }

      set
      {
        Assert.ArgumentNotNull(value, "class");
        this.attributes["class"] = value;
      }
    }

    public string Title
    {
      get
      {
        return this.GetAttribute("title");
      }

      set
      {
        Assert.ArgumentNotNull(value, "title");
        this.attributes["title"] = value;
      }
    }

    public string Target
    {
      get
      {
        return this.GetAttribute("target");
      }

      set
      {
        Assert.ArgumentNotNull(value, "target");
        this.attributes["target"] = value;
      }
    }

    public string QueryString
    {
      get
      {
        return this.GetAttribute("querystring");
      }

      set
      {
        Assert.ArgumentNotNull(value, "querystring");
        this.attributes["querystring"] = value;
      }
    }

    public ID TargetID
    {
      get
      {
        if (!this.attributes.ContainsKey("id"))
        {
          return ID.Null;
        }

        ID id;
        return ID.TryParse(this.attributes["id"], out id) ? id : ID.Null;
      }

      set
      {
        Assert.ArgumentNotNull(value, "id");
        this.attributes["id"] = value.ToString();
      }
    }

    public override string GetValue(string language, int version)
    {
      var value = base.GetValue(language, version);
      if (!string.IsNullOrEmpty(value) && !this.attributes.Any())
      {
        return value;
      }

      var link = new XElement("link");
      foreach (var de in this.attributes)
      {
        link.SetAttributeValue(de.Key, de.Value);
      }

      return link.ToString();
    }

    private string GetAttribute(string attribute)
    {
      return this.attributes.ContainsKey(attribute) ? this.attributes[attribute] : string.Empty;
    }
  }
}