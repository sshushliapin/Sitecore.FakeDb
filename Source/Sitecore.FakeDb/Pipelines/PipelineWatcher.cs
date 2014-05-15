namespace Sitecore.FakeDb.Pipelines
{
  using System;
  using System.Collections.Generic;
  using System.Xml;
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;
  using Sitecore.StringExtensions;
  using Sitecore.Xml;

  public class PipelineWatcher : IDisposable
  {
    private readonly XmlDocument config;

    private readonly IDictionary<string, PipelineArgs> expectedCalls = new Dictionary<string, PipelineArgs>();

    private readonly IDictionary<string, PipelineArgs> actualCalls = new Dictionary<string, PipelineArgs>();

    private readonly IDictionary<string, Func<PipelineArgs, bool>> checkThisArgs = new Dictionary<string, Func<PipelineArgs, bool>>();

    private readonly IDictionary<string, Func<PipelineArgs, bool>> filterThisArgs = new Dictionary<string, Func<PipelineArgs, bool>>();

    private IDictionary<string, Action<PipelineArgs>> processThisArgs = new Dictionary<string, Action<PipelineArgs>>();

    private string lastUsedPipelineName;

    private bool disposed;

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
      this.checkThisArgs.Add(pipelineName, checkThisArgs);
      this.Expects(pipelineName);
    }

    public virtual PipelineWatcher WhenCall(string pipelineName)
    {
      this.Expects(pipelineName);

      return this;
    }

    public virtual PipelineWatcher WithArgs(Func<PipelineArgs, bool> filterThisArgs)
    {
      this.filterThisArgs.Add(this.lastUsedPipelineName, filterThisArgs);

      return this;
    }

    public virtual void Then(Action<PipelineArgs> processThisArgs)
    {
      this.processThisArgs.Add(this.lastUsedPipelineName, processThisArgs);
    }

    public virtual void Expects(string pipelineName, PipelineArgs pipelineArgs)
    {
      Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");

      this.expectedCalls.Add(pipelineName, pipelineArgs);
      this.lastUsedPipelineName = pipelineName;

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

      foreach (var expectedCall in this.expectedCalls)
      {
        var expectedName = expectedCall.Key;
        Assert.IsTrue(this.actualCalls.ContainsKey(expectedName), MessageFormat, "pipelineName == \"" + expectedName + "\"");

        var expectedArgs = expectedCall.Value;
        if (expectedArgs != null)
        {
          Assert.IsTrue(expectedArgs == this.actualCalls[expectedName], MessageFormat, "pipelineArgs");
        }

        if (this.checkThisArgs.ContainsKey(expectedName))
        {
          Assert.IsTrue(this.checkThisArgs[expectedName](this.actualCalls[expectedName]), MessageFormat, "pipelineArgs");
        }
      }
    }

    protected virtual void OnPipelineRun(PipelineRunEventArgs e)
    {
      var pipelineName = e.PipelineName;

      // TODO:[High] Sometimes actualCalls might contain the key. Concurrency issue.
      if (actualCalls.ContainsKey(pipelineName))
      {
        this.actualCalls[pipelineName] = e.PipelineArgs;
      }
      else
      {
        this.actualCalls.Add(pipelineName, e.PipelineArgs);
      }

      if (this.filterThisArgs != null && this.processThisArgs.ContainsKey(pipelineName) && this.filterThisArgs[pipelineName](e.PipelineArgs))
      {
        this.processThisArgs[pipelineName](e.PipelineArgs);
      }
    }

    private void PipelineRun(object sender, PipelineRunEventArgs e)
    {
      this.OnPipelineRun(e);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (!disposing)
      {
        return;
      }

      PipelineWatcherProcessor.PipelineRun -= this.PipelineRun;
      this.disposed = true;
    }
  }
}