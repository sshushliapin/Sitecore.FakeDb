namespace Sitecore.FakeDb.Tests
{
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;

  public class  DefaultAutoDataAttribute : AutoDataAttribute
  {
    public DefaultAutoDataAttribute()
    {
      var database = Database.GetDatabase("master");
      this.Fixture.Inject(database);
      this.Fixture.Inject(Substitute.For<DataStorage>(database));
      this.Fixture.Register(ItemHelper.CreateInstance);
      this.Fixture.Register(() => Language.Parse("en"));
    }
  }

  public class InlineDefaultAutoDataAttribute : InlineAutoDataAttribute
  {
    public InlineDefaultAutoDataAttribute(params object[] values)
      : base(new DefaultAutoDataAttribute(), values)
    {
    }
  }
}