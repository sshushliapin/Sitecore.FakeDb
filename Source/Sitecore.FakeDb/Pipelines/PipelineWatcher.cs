namespace Sitecore.FakeDb.Pipelines
{
  using System.Xml;
  using Sitecore.StringExtensions;
  using Sitecore.Xml;
  using System;

  public class PipelineWatcher : IDisposable
  {
    private readonly XmlDocument config;

    private string pipelineName;

    private string calledPipeline;

    public PipelineWatcher(XmlDocument config)
    {
      this.config = config;
      PipelineWatcherProcessor.PipelineRun += PipelineRun;
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

      var type = typeof(PipelineWatcherProcessor);
      var value = type + ", " + type.Assembly.GetName().Name;
      XmlUtil.AddAttribute("type", value, processorNode);

      var paramXml = "<param desc=\"pipelineName\">{0}</param>".FormatWith(pipelineName);
      XmlUtil.AddXml(paramXml, processorNode);
    }

    public virtual void EnsureExpectations()
    {
      const string MessageFormat = "Expected to receive a pipeline call matching (pipelineName == \"{0}\"). Actually received no matching calls.";
      Diagnostics.Assert.IsTrue(this.pipelineName == this.calledPipeline, MessageFormat, this.pipelineName);
    }

    protected virtual void OnPipelineRun(PipelineRunEventArgs e)
    {
      this.calledPipeline = e.PipelineName;
    }

    private void PipelineRun(object sender, PipelineRunEventArgs e)
    {
      this.OnPipelineRun(e);
    }

    public virtual void Dispose()
    {
      PipelineWatcherProcessor.PipelineRun -= this.PipelineRun;
    }
  }
}