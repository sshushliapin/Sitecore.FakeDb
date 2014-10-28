namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  using Sitecore.Pipelines;

  public class SetContextIsUnitTesting
  {
    public virtual void Process(PipelineArgs args)
    {
      Context.IsUnitTesting = true;
    }
  }
}