namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System.Reflection;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;

  public class ContentAttribute : CustomizeAttribute
  {
    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
      return new ContentItemCustomization();
    }
  }
}