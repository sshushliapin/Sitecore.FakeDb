namespace Sitecore.FakeDb.Tests.Pipelines.GetTranslation
{
  using Sitecore.Pipelines.GetTranslation;

  public class GetFakeTranslation
  {
    public void Process(GetTranslationArgs args)
    {
      args.Result = args.Key + "*";
    }
  }
}