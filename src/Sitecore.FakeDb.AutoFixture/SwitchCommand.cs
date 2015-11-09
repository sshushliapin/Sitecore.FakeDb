namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Common;
  using Sitecore.Diagnostics;

  public class SwitchCommand : ISpecimenCommand
  {
    public void Execute(object specimen, ISpecimenContext context)
    {
      Assert.ArgumentNotNull(specimen, "specimen");
      Assert.ArgumentNotNull(context, "context");

      if (specimen == null)
      {
        return;
      }

      // TODO: Should be done by AutoFixture.
      var type = specimen.GetType();
      if (type.Namespace == "Castle.Proxies")
      {
        type = type.BaseType;
      }

      // TODO: How to exit the switcher?
      typeof(Switcher<,>)
        .MakeGenericType(type, type)
        .GetMethod("Enter")
        .Invoke(null, new[] { specimen });
    }
  }
}