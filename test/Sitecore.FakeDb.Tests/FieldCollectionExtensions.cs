namespace Sitecore.FakeDb
{
  using System;
  using System.Linq;
  using System.Text;
  using Sitecore.Collections;
  using Sitecore.Data.Fields;

  internal static class FieldCollectionExtensions
  {
    internal static string ToDbgString(this FieldCollection fields)
    {
      var sb = new StringBuilder();
      AppendFieldInfo(sb, fields, "Fields with values:", x => !string.IsNullOrEmpty(x.Value));
      AppendFieldInfo(sb, fields, "Empty fields:", x => string.IsNullOrEmpty(x.Value));

      return sb.ToString();
    }

    internal static void ToConsoleOut(this FieldCollection fields)
    {
      Console.Out.WriteLine("fields = {0}", fields.ToDbgString());
    }

    private static void AppendFieldInfo(StringBuilder sb, FieldCollection fields, string header, Func<Field, bool> predicate)
    {
      sb.AppendLine(string.Empty);
      sb.AppendLine(header);
      foreach (var field in fields.Where(predicate))
      {
        sb.AppendLine(string.Format("\"{0}\":\"{1}\"", field.Name, field.Value));
      }
    }
  }
}