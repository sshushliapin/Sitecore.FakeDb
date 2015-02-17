namespace Sitecore.FakeDb.NSubstitute
{
  using System;
  using System.Configuration.Provider;
  using global::NSubstitute;
  using Sitecore.Diagnostics;
  using Sitecore.Reflection;

  public class NSubstituteFactory : IFactory
  {
    public object GetObject(string typeName)
    {
      Assert.ArgumentNotNullOrEmpty(typeName, "typeName");

      var type = Type.GetType(typeName, true);
      var obj = Substitute.For(new[] { type }, new object[] { });

      var provider = obj as ProviderBase;
      if (provider != null)
      {
        provider.Name.Returns(type.Name);
      }

      return obj;
    }
  }
}