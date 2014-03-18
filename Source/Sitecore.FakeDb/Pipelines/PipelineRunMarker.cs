namespace Sitecore.FakeDb.Pipelines
{
  using System.Xml;
  using Sitecore.Configuration;
  using Sitecore.Pipelines;
  using Sitecore.Xml;

  public class PipelineRunMarker
  {
    private readonly string pipelineName;

    private XmlDocument config;

    public PipelineRunMarker(string pipelineName)
    {
      Diagnostics.Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");

      this.pipelineName = pipelineName;
    }

    public string PipelineName
    {
      get { return this.pipelineName; }
    }

    public XmlDocument Config
    {
      get
      {
        return this.config ?? (this.config = Factory.GetConfiguration());
      }

      set
      {
        Diagnostics.Assert.ArgumentNotNull(value, "value");

        this.config = value;
      }
    }

    public virtual void Process(PipelineArgs args)
    {
      var path = "/sitecore/pipelines/" + this.pipelineName;
      var node = XmlUtil.EnsurePath(path, this.Config);

      XmlUtil.AddAttribute("isRun", "true", node);
    }
  }
}