namespace Sitecore.FakeDb
{
  using Sitecore.Collections;

  internal class DbLanguages
  {
    private readonly LanguageCollection languages;

    public DbLanguages(LanguageCollection languages)
    {
      this.languages = languages;
    }

    public LanguageCollection GetLanguages()
    {
      return this.languages;
    }
  }
}