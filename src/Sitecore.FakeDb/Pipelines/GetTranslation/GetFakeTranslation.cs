namespace Sitecore.FakeDb.Pipelines.GetTranslation
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

            var language = args.Language.Name;

            var prefix = Settings.GetSetting("FakeDb.AutoTranslatePrefix");
            prefix = prefix.Replace(@"{lang}", language);

            var suffix = Settings.GetSetting("FakeDb.AutoTranslateSuffix");

            if (string.IsNullOrEmpty(prefix) && string.IsNullOrEmpty(suffix))
            {
                suffix = "*";
            }
            else if (suffix.Contains(@"{lang}"))
            {
                suffix = suffix.Replace(@"{lang}", language);
            }

            if (!string.IsNullOrEmpty(suffix) && args.Key.EndsWith(suffix))
            {
                return;
            }

            args.Result = prefix + args.Key + suffix;
        }
    }
}