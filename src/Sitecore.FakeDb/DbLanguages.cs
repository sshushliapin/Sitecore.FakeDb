namespace Sitecore.FakeDb
{
    using Sitecore.Collections;
    using Sitecore.Common;
    using Sitecore.Diagnostics;
    using Sitecore.FakeDb.Data.DataProviders;
    using Sitecore.Globalization;

    /// <summary>
    /// Encapsulates a list of languages to be switched using 
    /// <see cref="Switcher{T}"/> for the <see cref="SwitchingLanguageDataProvider"/>.
    /// </summary>
    internal class DbLanguages
    {
        private readonly LanguageCollection languages;

        public DbLanguages(params Language[] languages)
        {
            Assert.ArgumentNotNull(languages, "languages");

            this.languages = new LanguageCollection(languages);
        }

        public LanguageCollection GetLanguages()
        {
            return this.languages;
        }
    }
}