using Sitecore.Data.Serialization.ObjectModel;

namespace Sitecore.FakeDb.Serialization
{
    /// <summary>
    /// Abstraction that is needed because we want to share functionality for building items as well as templates.
    /// </summary>
    public interface IDsDbItem
    {
        DbFieldCollection Fields { get; }

        void Add(DbField field);
    }
}
