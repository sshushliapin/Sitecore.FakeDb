namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using global::AutoFixture;
  using global::AutoFixture;
  using global::AutoFixture.Xunit2;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class ContentAttributeTest
  {
    [Fact]
    public void ShouldBeCustomizeAttribute()
    {
      var sut = new ContentAttribute();
      sut.Should().BeAssignableTo<Attribute>();
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

    [Fact]
    public void ShouldBePropertyAttribute()
    {
      typeof(ContentAttribute).GetCustomAttributes(false).Should().BeEquivalentTo(new AttributeUsageAttribute(AttributeTargets.Parameter));
    }

    private class AutoDbDataAttribute : AutoDataAttribute
    {
      public AutoDbDataAttribute()
        : base(() => new Fixture().Customize(new AutoDbCustomization())) { }
    }
  }
}