namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class ContentAttributeTest
  {
    [Fact]
    public void ShouldBeAttribute()
    {
      new ContentAttribute().Should().BeAssignableTo<Attribute>();
    }

    [Fact]
    public void ShouldBeParameterAttribute()
    {
      typeof(ContentAttribute).GetCustomAttributes(false).Should().BeEquivalentTo(new AttributeUsageAttribute(AttributeTargets.Parameter));
    }

    [Theory, AutoDbData]
    public void ShouldAddDbItem(Db db, [Content]DbItem item)
    {
      db.GetItem(item.ID).Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldNotAddItemsNotMarkedAsContent(Db db, [Content]DbItem item, DbItem foreigner)
    {
      db.GetItem(foreigner.ID).Should().BeNull();
    }

    private class AutoDbDataAttribute : AutoDataAttribute
    {
      public AutoDbDataAttribute()
        : base(new Fixture().Customize(new AutoDbCustomization())) { }
    }
  }
}