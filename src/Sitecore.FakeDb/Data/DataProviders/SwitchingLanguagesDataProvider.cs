namespace Sitecore.FakeDb.Data.DataProviders
{
  using Sitecore.Collections;
  using Sitecore.Common;
  using Sitecore.Data.DataProviders;
  using CallContext = Sitecore.Data.DataProviders.CallContext;

  public sealed class SwitchingLanguagesDataProvider : DataProvider
  {
    public override LanguageCollection GetLanguages(CallContext context)
    {
      return Switcher<DbLanguages>.CurrentValue == null
        ? new LanguageCollection()
        : Switcher<DbLanguages>.CurrentValue.GetLanguages();
    }
  }
}