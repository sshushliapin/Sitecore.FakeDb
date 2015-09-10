namespace Sitecore.FakeDb.Tests
{
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;

  public class DefaultAutoDataAttribute : AutoDataAttribute
  {
    public DefaultAutoDataAttribute()
      : base(new Fixture().Customize(new DefaultConventions()))
    {
    }
  }

  public class InlineDefaultAutoDataAttribute : InlineAutoDataAttribute
  {
    public InlineDefaultAutoDataAttribute(params object[] values)
      : base(new DefaultAutoDataAttribute(), values)
    {
    }
  }

  internal class DefaultConventions : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      var database = Database.GetDatabase("master");
      fixture.Inject(database);
      fixture.Inject(Substitute.For<DataStorage>(database));
      fixture.Register(ItemHelper.CreateInstance);
      fixture.Register(() => Language.Parse("en"));
    }
  }
}