namespace Sitecore.FakeDb.Pipelines.GetTranslation
{
  using Sitecore.Pipelines.GetTranslation;

  public class GetFakeTranslation
  {
    public void Process(GetTranslationArgs args)
    {
      args.Result = args.Language.Name + ":" + args.Key;
    }
  }
}