namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
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
      item.Fields.Add(new DbField("__Revision", FieldIDs.Revision) { Value = ID.NewID.ToString() });
      item.Fields.Add(new DbField("__Updated", FieldIDs.Updated) { Value = date });
      item.Fields.Add(new DbField("__Updated by", FieldIDs.UpdatedBy) { Value = user });
    }
  }
}