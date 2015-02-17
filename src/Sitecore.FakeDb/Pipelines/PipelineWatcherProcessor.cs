namespace Sitecore.FakeDb.Pipelines
{
  using System;
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public class PipelineWatcherProcessor
  {
    public static event EventHandler<PipelineRunEventArgs> PipelineRun;

    private readonly string pipelineName;

    public PipelineWatcherProcessor(string pipelineName)
    {
      Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");

      this.pipelineName = pipelineName;
    }

    public string PipelineName
    {
      get { return this.pipelineName; }
    }

    public void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (PipelineRun != null)
      {
        PipelineRun(this, new PipelineRunEventArgs(this.PipelineName, args));
      }
    }
  }
}