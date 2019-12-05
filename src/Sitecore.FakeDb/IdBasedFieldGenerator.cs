namespace Sitecore.FakeDb
{
    using Sitecore.Data;

    /// <summary>
    /// Generates a <see cref="FieldInfo"/> to be used in the <see cref="DbField"/> creation based on the
    /// predefined <see cref="ID"/> and auto-generated name.
    /// </summary>
    public class IdBasedFieldGenerator : IDbFieldBuilder
    {
        public FieldInfo Build(object request)
        {
            var id = request as ID;
            return !ID.IsNullOrEmpty(id) ? new FieldInfo(id.ToShortID().ToString(), id, false, "text") : FieldInfo.Empty;
        }
    }
}