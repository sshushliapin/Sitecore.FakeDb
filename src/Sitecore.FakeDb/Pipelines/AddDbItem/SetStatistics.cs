namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
    using System.Linq;
    using Sitecore.Data;

    /// <summary>
    /// Creates and fulfills item statistics fields for all the item languages.
    /// The fields included are 'Created', 'CreatedBy', 'Revision', 'Updated' 
    /// and 'UpdatedBy'.
    /// <para>
    /// The 'Created' and 'Updated' fields are set to the current date in ISO 
    /// format. The 'CreatedBy' and 'UpdatedBy' fields store the current user 
    /// name. The 'Revision' field is a <see cref="System.Guid"/> generated for
    /// each of the item languages.
    /// </para>
    /// </summary>
    public class SetStatistics
    {
        public virtual void Process(AddDbItemArgs args)
        {
            var item = args.DbItem;
            var date = DateUtil.IsoNow;
            var user = Context.User.Name;

            AddStatisticsFields(item, date, user);
            SetStatisticsForAllLanguages(item, date, user);
        }

        private static void AddStatisticsFields(DbItem item, string date, string user)
        {
            item.Fields.Add(new DbField("__Created", FieldIDs.Created) {Value = date});
            item.Fields.Add(new DbField("__Created by", FieldIDs.CreatedBy) {Value = user});
            item.Fields.Add(new DbField("__Revision", FieldIDs.Revision) {Value = ID.NewID.ToString()});
            item.Fields.Add(new DbField("__Updated", FieldIDs.Updated) {Value = date});
            item.Fields.Add(new DbField("__Updated by", FieldIDs.UpdatedBy) {Value = user});
        }

        private static void SetStatisticsForAllLanguages(DbItem item, string date, string user)
        {
            foreach (var lang in item.Fields
                .SelectMany(field => field.Values)
                .Select(l => l.Key))
            {
                SetFieldValue(item.Fields[FieldIDs.Created], lang, date);
                SetFieldValue(item.Fields[FieldIDs.CreatedBy], lang, user);
                SetFieldValue(item.Fields[FieldIDs.Revision], lang, ID.NewID.ToString());
                SetFieldValue(item.Fields[FieldIDs.Updated], lang, date);
                SetFieldValue(item.Fields[FieldIDs.UpdatedBy], lang, user);
            }
        }

        private static void SetFieldValue(DbField field, string lang, string value)
        {
            var valueSet = !string.IsNullOrEmpty(
                field.GetValue(lang, Version.Latest.Number));

            if (!valueSet)
            {
                field.SetValue(lang, value);
            }
        }
    }
}