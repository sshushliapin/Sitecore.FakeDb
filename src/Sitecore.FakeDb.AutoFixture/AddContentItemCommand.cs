namespace Sitecore.FakeDb.AutoFixture
{
    using global::AutoFixture.Kernel;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;

    /// <summary>
    /// A command that adds items to the current <see cref="Database"/>.
    /// The default <see cref="Database"/> is "master".
    /// </summary>
    public class AddContentItemCommand : ISpecimenCommand
    {
        public void Execute(object specimen, ISpecimenContext context)
        {
            Assert.ArgumentNotNull(specimen, "specimen");
            Assert.ArgumentNotNull(context, "context");

            var item = specimen as Item;
            if (item == null)
            {
                return;
            }

            var db = (Db) context.Resolve(typeof(Db));
            db.Add(new DbItem(item.Name, item.ID, item.TemplateID) {BranchId = item.BranchId});
        }
    }
}