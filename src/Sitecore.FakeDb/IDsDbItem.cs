using System.IO;
using Sitecore.Data.Serialization.ObjectModel;

namespace Sitecore.FakeDb
{
    /// <summary>
    /// Abstraction that is needed because we want to share functionality for building items as well as templates.
    /// </summary>
    public interface IDsDbItem
    {
#pragma warning disable 618
        SyncItem SyncItem { get; }
#pragma warning restore 618

        FileInfo File { get; }

        DbFieldCollection Fields { get; }

        void Add(DbField field);
    }
}
