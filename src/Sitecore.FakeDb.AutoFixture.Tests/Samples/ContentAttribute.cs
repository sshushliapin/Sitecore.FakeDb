namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
  using System.Reflection;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;

  internal class ContentAttribute : CustomizeAttribute
  {
    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
      return new ContentItemCustomization();
    }
  }
}