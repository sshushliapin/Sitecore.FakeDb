namespace Sitecore.FakeDb
{
    using Sitecore.Data;

    /// <summary>
    /// Generates a <see cref="FieldInfo"/> to be used in the <see cref="DbField"/> creation based on the
    /// predefined name and auto-generated <see cref="ID"/>.
    /// </summary>
    public class NameBasedFieldGenerator : IDbFieldBuilder
    {
        public FieldInfo Build(object request)
        {
            var name = request as string;
            return !string.IsNullOrWhiteSpace(name) ? new FieldInfo(name, ID.NewID, false, "text") : FieldInfo.Empty;
        }
    }
}