namespace Sitecore.FakeDb.Pipelines
{
  using System;
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public class PipelineRunEventArgs : EventArgs
  {
    public PipelineRunEventArgs(string pipelineName, PipelineArgs pipelineArgs)
    {
      Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");
      Assert.ArgumentNotNull(pipelineArgs, "pipelineArgs");

      this.PipelineName = pipelineName;
      this.PipelineArgs = pipelineArgs;
    }

    public string PipelineName { get; private set; }

    public PipelineArgs PipelineArgs { get; private set; }
  }
}
