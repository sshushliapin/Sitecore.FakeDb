namespace Sitecore.FakeDb.Data.DataProviders
{
    using Sitecore.Collections;
    using Sitecore.Common;
    using Sitecore.Data;
    using Sitecore.Data.DataProviders;
    using CallContext = Sitecore.Data.DataProviders.CallContext;

    /// <summary>
    /// Represents a <see cref="DataProvider" /> used to return a list of 
    /// dynamically configured <see cref="Database"/> languages retrieved from 
    /// the <see cref="Switcher{DbLanguages}"/> class.
    /// </summary>
    public sealed class SwitchingLanguageDataProvider : DataProvider
    {
        /// <summary>
        /// Gets a list of <see cref="Database"/> languages.
        /// </summary>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// Empty <see cref="LanguageCollection"/> if no <see cref="DbLanguages"/>
        /// set via <see cref="Switcher{DbLanguages}"/>; otherwise the list of
        /// configured languages.
        /// </returns>
        public override LanguageCollection GetLanguages(CallContext context)
        {
            return Switcher<DbLanguages>.CurrentValue == null
              ? new LanguageCollection()
              : Switcher<DbLanguages>.CurrentValue.GetLanguages();
        }
    }
}