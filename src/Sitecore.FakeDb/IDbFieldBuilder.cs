namespace Sitecore.FakeDb
{
  public interface IDbFieldBuilder
  {
    FieldInfo Build(object request);
  }
}