namespace Sitecore.FakeDb.AutoFixture
{
  using System.Reflection;
  using Ploeh.AutoFixture.Kernel;

  public class DbItemParameterSpecification : IRequestSpecification
  {
    public bool IsSatisfiedBy(object request)
    {
      var parameterInfo = request as ParameterInfo;
      if (parameterInfo == null)
      {
        return false;
      }

      return new DbItemSpecification().IsSatisfiedBy(parameterInfo.ParameterType);
    }
  }
}