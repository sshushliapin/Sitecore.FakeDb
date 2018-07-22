namespace Sitecore.FakeDb.AutoFixture
{
  using System;
  using global::AutoFixture.Kernel;
  using Sitecore.Data.Items;

  public class TemplateItemSpecification : IRequestSpecification
  {
    public bool IsSatisfiedBy(object request)
    {
      return typeof(TemplateItem) == request as Type;
    }
  }
}