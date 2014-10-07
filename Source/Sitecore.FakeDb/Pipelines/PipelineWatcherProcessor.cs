namespace Sitecore.FakeDb.Pipelines
{
  using System;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.DataProviders;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Pipelines;

  public class PipelineWatcherProcessor
  {
    public static event EventHandler<PipelineRunEventArgs> PipelineRun;

    private readonly string pipelineName;

    public PipelineWatcherProcessor(string pipelineName)
    {
      Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");

      this.pipelineName = pipelineName;

      // TODO:[High] Open question: how to pass the DataStorage to this class?
      if (this.DataStorage == null)
      {
        this.DataStorage = ((FakeDataProvider)Database.GetDatabase("master").GetDataProviders()[0]).DataStorage;
      }
    }

    public string PipelineName
    {
      get { return this.pipelineName; }
    }

    public DataStorage DataStorage { get; set; }

    public virtual void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (PipelineRun != null)
      {
        PipelineRun(this, new PipelineRunEventArgs(this.PipelineName, args));
      }

      if (this.DataStorage != null && this.DataStorage.Pipelines.ContainsKey(this.PipelineName))
      {
        this.DataStorage.Pipelines[this.PipelineName].Process(args);
      }
    }
  }
}