namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
    using System;
    using Sitecore.Globalization;

    public abstract class AutoTranslateTestBase : IDisposable
    {
        protected AutoTranslateTestBase()
        {
            this.Db = new Db();
            this.Language = Language.Parse("da");
        }

        protected Db Db { get; private set; }

        protected Language Language { get; private set; }

        public void Dispose()
        {
            this.Db.Dispose();
        }
    }
}