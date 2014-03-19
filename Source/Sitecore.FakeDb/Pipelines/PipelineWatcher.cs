namespace Sitecore.FakeDb.Pipelines
{
  using System;
  using System.Xml;
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;
  using Sitecore.StringExtensions;
  using Sitecore.Xml;

  public class PipelineWatcher : IDisposable
  {
    private readonly XmlDocument config;

    private string expectedName;

    private string calledPipeline;

    private PipelineArgs expectedArgs;

    private PipelineArgs calledArgs;

    private Func<PipelineArgs, bool> checkThisArgs;

    private Func<PipelineArgs, bool> filterThisArgs;

    private Action<PipelineArgs> processThisArgs;

    public PipelineWatcher(XmlDocument config)
    {
      Assert.ArgumentNotNull(config, "config");

      this.config = config;

      PipelineWatcherProcessor.PipelineRun += this.PipelineRun;
    }

    protected internal XmlDocument ConfigSection
    {
      get { return this.config; }
    }

    public virtual void Expects(string pipelineName)
    {
      this.Expects(pipelineName, (PipelineArgs)null);
    }

    public virtual void Expects(string pipelineName, Func<PipelineArgs, bool> checkThisArgs)
    {
      this.checkThisArgs = checkThisArgs;
      this.Expects(pipelineName);
    }

    public virtual PipelineWatcher WhenCall(string pipelineName)
    {
      this.Expects(pipelineName);

      return this;
    }

    public virtual PipelineWatcher WithArgs(Func<PipelineArgs, bool> filterThisArgs)
    {
      this.filterThisArgs = filterThisArgs;

      return this;
    }

    public virtual void Then(Action<PipelineArgs> processThisArgs)
    {
      this.processThisArgs = processThisArgs;
    }

    public virtual void Expects(string pipelineName, PipelineArgs pipelineArgs)
    {
      Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");

      this.expectedName = pipelineName;
      this.expectedArgs = pipelineArgs;


      var path = "/sitecore/pipelines/" + pipelineName + "/processor";
      var processorNode = XmlUtil.EnsurePath(path, this.config);

      var type = typeof(PipelineWatcherProcessor);
      var value = type + ", " + type.Assembly.GetName().Name;
      XmlUtil.AddAttribute("type", value, processorNode);

      var paramXml = "<param desc=\"expectedName\">{0}</param>".FormatWith(pipelineName);
      XmlUtil.AddXml(paramXml, processorNode);
    }

    public virtual void EnsureExpectations()
    {
      const string MessageFormat = "Expected to receive a pipeline call matching ({0}). Actually received no matching calls.";
      Assert.IsTrue(this.expectedName == this.calledPipeline, MessageFormat, "pipelineName == \"" + this.expectedName + "\"");

      if (this.expectedArgs != null)
      {
        Assert.IsTrue(this.expectedArgs == this.calledArgs, MessageFormat, "pipelineArgs");
      }

      if (this.checkThisArgs != null)
      {
        Assert.IsTrue(this.checkThisArgs(this.calledArgs), MessageFormat, "pipelineArgs");
      }
    }

    protected virtual void OnPipelineRun(PipelineRunEventArgs e)
    {
      this.calledPipeline = e.PipelineName;
      this.calledArgs = e.PipelineArgs;

      if (this.filterThisArgs != null && this.processThisArgs != null && this.filterThisArgs(this.calledArgs))
      {
        this.processThisArgs(this.calledArgs);
      }
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