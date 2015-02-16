namespace Sitecore.FakeDb.Pipelines
{
  using System;
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public class PipelineWatcherProcessor
  {
    public static event EventHandler<PipelineRunEventArgs> PipelineRun;

    private readonly string pipelineName;

    private readonly string databaseName;

    public PipelineWatcherProcessor(string pipelineName)
    {
      Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");

      this.pipelineName = pipelineName;
    }

    public PipelineWatcherProcessor(string pipelineName, string databaseName)
      : this(pipelineName)
    {
      Assert.ArgumentNotNullOrEmpty(databaseName, "databaseName");

      this.databaseName = databaseName;
    }

    public string PipelineName
    {
      get { return this.pipelineName; }
    }

    public string DatabaseName
    {
      get { return this.databaseName; }
    }

    public virtual void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (PipelineRun != null)
      {
        PipelineRun(this, new PipelineRunEventArgs(this.PipelineName, args));
      }
    }
  }
}