namespace Sitecore.FakeDb
{
  using Sitecore.Data;

  internal class DbFieldBuilder
  {
    private static readonly FieldInfoReference Fields = new FieldInfoReference();

    public void Build(DbField field)
    {
      var fieldInfo = new FieldInfo();
      if (!string.IsNullOrEmpty(field.Name) && field.Name.StartsWith("__"))
      {
        fieldInfo = Fields[field.Name];
      }

      if (fieldInfo == FieldInfo.Empty && field.ID != (ID)null)
      {
        fieldInfo = Fields[field.ID.Guid];
      }

      if (fieldInfo == FieldInfo.Empty)
      {
        if (ID.IsNullOrEmpty(field.ID))
        {
          field.ID = ID.NewID;
        }

        if (string.IsNullOrEmpty(field.Name) && field.ID != (ID)null)
        {
          field.Name = field.ID.ToShortID().ToString();
        }
      }
      else
      {
        field.ID = ID.Parse(fieldInfo.Id);
        field.Name = fieldInfo.Name;
        field.Shared = fieldInfo.Shared;
        field.Type = fieldInfo.Type;
      }
    }
  }
}