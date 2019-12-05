namespace Sitecore.FakeDb.AutoFixture
{
    using System.Reflection;
    using global::AutoFixture.Kernel;

    public class ItemParameterSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            var parameterInfo = request as ParameterInfo;
            if (parameterInfo == null)
            {
                return false;
            }

            return new ItemSpecification().IsSatisfiedBy(parameterInfo.ParameterType);
        }
    }
}