namespace Sitecore.FakeDb.Pipelines.GetTranslation
{
  using Sitecore.Configuration;
  using Sitecore.Pipelines.GetTranslation;

  public class GetFakeTranslation
  {
    public void Process(GetTranslationArgs args)
    {
      if (!Settings.GetBoolSetting("Sitecore.FakeDb.AutoTranslate", false))
      {
        return;
      }

      var prefix = Settings.GetSetting("Sitecore.FakeDb.AutoTranslatePrefix");
      prefix = prefix.Replace(@"{lang}", Context.Language.Name);

      var suffix = Settings.GetSetting("Sitecore.FakeDb.AutoTranslateSuffix");
      suffix = suffix.Replace(@"{lang}", Context.Language.Name);
      if (args.Key.EndsWith(suffix))
      {
        return;
      }

      args.Result = prefix + args.Key + suffix;
    }
  }
}