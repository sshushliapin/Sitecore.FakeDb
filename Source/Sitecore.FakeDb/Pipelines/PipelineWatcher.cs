namespace Sitecore.FakeDb.Pipelines
{
  using System.Xml;
  using Sitecore.StringExtensions;
  using Sitecore.Xml;

  public class PipelineWatcher
  {
    private readonly XmlDocument config;

    private string pipelineName;

    public PipelineWatcher(XmlDocument config)
    {
      this.config = config;
    }

    public XmlDocument ConfigSection
    {
      get { return this.config; }
    }

    public virtual void Expects(string pipelineName)
    {
      Diagnostics.Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");

      this.pipelineName = pipelineName;

      var path = "/sitecore/pipelines/" + pipelineName + "/processor";
      var processorNode = XmlUtil.EnsurePath(path, this.config);

      var type = typeof(PipelineRunMarker);
      var value = type + ", " + type.Assembly.GetName().Name;
      XmlUtil.AddAttribute("type", value, processorNode);

      var paramXml = "<param desc=\"pipelineName\">{0}</param>".FormatWith(pipelineName);
      XmlUtil.AddXml(paramXml, processorNode);
    }

    public virtual void EnsureExpectations()
    {
      var path = "/sitecore/pipelines/" + this.pipelineName;

      var marker = this.config.SelectSingleNode(path);
      Diagnostics.Assert.IsNotNull(marker, "marker");

      var isRun = XmlUtil.GetAttribute("isRun", marker, "false");

      const string MessageFormat = "Expected to receive a pipeline call matching (pipelineName == \"{0}\"). Actually received no matching calls.";
      Diagnostics.Assert.IsTrue(MainUtil.GetBool(isRun, false), MessageFormat, this.pipelineName);
    }
  }
}