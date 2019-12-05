namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;

    internal class AutoDbDataAttribute : AutoDataAttribute
    {
        public AutoDbDataAttribute()
            : base(() => new Fixture().Customize(new AutoDbCustomization()))
        {
        }
    }
}