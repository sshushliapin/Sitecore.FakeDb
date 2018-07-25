namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using global::AutoFixture.Kernel;
  using global::AutoFixture.Xunit2;
  using Sitecore.Data.Items;
  using Xunit;

  public class TemplateItemSpecificationTest
  {
    [Theory, AutoData]
    public void SutIsRequestSpecification(TemplateItemSpecification sut)
    {
      sut.Should().BeAssignableTo<IRequestSpecification>();
    }

    [Theory, AutoData]
    public void IsSatisfiedByReturnsFalseIfRequestIsNull(TemplateItemSpecification sut)
    {
      sut.IsSatisfiedBy(null).Should().BeFalse();
    }

    [Theory, AutoData]
    public void IsSatisfiedByReturnsTrueFalseIfRequestIsTemplateItem(TemplateItemSpecification sut)
    {
      sut.IsSatisfiedBy(typeof(TemplateItem)).Should().BeTrue();
    }
  }
}