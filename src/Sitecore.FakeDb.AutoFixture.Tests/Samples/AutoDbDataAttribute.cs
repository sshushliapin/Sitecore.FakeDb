namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;

  internal class AutoDbDataAttribute : AutoDataAttribute
  {
    public AutoDbDataAttribute()
      : base(new Fixture().Customize(new AutoDbCustomization()))
    {
    }
  }
}