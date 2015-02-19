namespace Sitecore.FakeDb
{
  using System;
  using Sitecore.Data;
  using Sitecore.Reflection;

  public class DbFactory : IFactory
  {
    private static readonly ID id = ID.NewID;

    public object GetObject(string identifier)
    {
      Console.Out.WriteLine("id = {0}", id);

      var type = ReflectionUtil.GetTypeInfo(identifier);
      var obj = Activator.CreateInstance(type);



      return obj;
    }
  }
}