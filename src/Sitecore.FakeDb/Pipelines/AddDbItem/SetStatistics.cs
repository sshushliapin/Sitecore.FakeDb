namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using System.Linq;
  using Sitecore.Data;

  public class SetStatistics
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;

      var date = DateUtil.IsoNow;
      var user = Context.User.Name;

      item.Fields.Add(new DbField("__Created", FieldIDs.Created) { Value = date });
      item.Fields.Add(new DbField("__Created by", FieldIDs.CreatedBy) { Value = user });
      item.Fields.Add(new DbField("__Revision", FieldIDs.Revision));
      item.Fields.Add(new DbField("__Updated", FieldIDs.Updated) { Value = date });
      item.Fields.Add(new DbField("__Updated by", FieldIDs.UpdatedBy) { Value = user });

      SetRevisionForAllLanguages(item);
    }

    private static void SetRevisionForAllLanguages(DbItem item)
    {
      foreach (var lang in item.Fields.SelectMany(field => field.Values))
      {
        var revisionField = item.Fields[FieldIDs.Revision];
        var revisionSet = !string.IsNullOrEmpty(
          revisionField.GetValue(lang.Key, Version.Latest.Number));
        if (!revisionSet)
        {
          revisionField.SetValue(lang.Key, ID.NewID.ToString());
        }
      }
    }
  }
}