﻿namespace Sitecore.FakeDb.Pipelines.GetTranslation
{
  using Sitecore.Configuration;
  using Sitecore.Pipelines.GetTranslation;

  public class GetFakeTranslation
  {
    public void Process(GetTranslationArgs args)
    {
      if (!Settings.GetBoolSetting("FakeDb.AutoTranslate", false))
      {
        return;
      }

      var prefix = Settings.GetSetting("FakeDb.AutoTranslatePrefix");
      prefix = prefix.Replace(@"{lang}", Context.Language.Name);

      var suffix = Settings.GetSetting("FakeDb.AutoTranslateSuffix");
      suffix = suffix.Replace(@"{lang}", Context.Language.Name);
      if (!string.IsNullOrEmpty(suffix) && args.Key.EndsWith(suffix))
      {
        return;
      }

      args.Result = prefix + args.Key + suffix;
    }
  }
}