namespace Sitecore.FakeDb
{
  using Sitecore.Diagnostics;

  public class DbFieldBuilder
  {
    public static readonly DbFieldBuilder Default;

    static DbFieldBuilder()
    {
      var fields = new FieldInfoReference();
      Default = new DbFieldBuilder(
                  new CompositeFieldBuilder(
                    new MixedFieldBuilder(
                      new StandardNameFieldBuilder(fields)),
                    new MixedFieldBuilder(
                      new StandardIdFieldBuilder(fields)),
                    new IdNameFieldBuilder(
                      new RandomNameFieldBuilder(),
                      new RandomIdFieldBuilder())));
    }

    public DbFieldBuilder(IDbFieldBuilder builder)
    {
      this.Builder = builder;
    }

    public IDbFieldBuilder Builder { get; private set; }

    public static DbFieldBuilder FromName()
    {
      return new DbFieldBuilder(
        new CompositeFieldBuilder(
          new StandardNameFieldBuilder(new FieldInfoReference()),
          new RandomNameFieldBuilder()));
    }


    public static DbFieldBuilder FromId()
    {
      return new DbFieldBuilder(
        new CompositeFieldBuilder(
          new StandardIdFieldBuilder(new FieldInfoReference()),
          new RandomIdFieldBuilder()));
    }

    public void Build(object request, DbField field)
    {
      Assert.ArgumentNotNull(field, "field");

      var fieldInfo = this.Builder.Build(request);

      field.ID = fieldInfo.Id;
      field.Name = fieldInfo.Name;
      field.Shared = fieldInfo.Shared;
      field.Type = fieldInfo.Type;
    }
  }
}