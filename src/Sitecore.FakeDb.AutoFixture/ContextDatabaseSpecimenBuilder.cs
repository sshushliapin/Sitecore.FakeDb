namespace Sitecore.FakeDb.AutoFixture
{
    using global::AutoFixture.Kernel;
    using Sitecore.Data;

    public class ContextDatabaseSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(Database).Equals(request))
            {
                return new NoSpecimen();
            }

            var database = Context.Database;
            if (database == null)
            {
                return new NoSpecimen();
            }

            return new DatabaseSpecimenBuilder(database.Name).Create(request, context);
        }
    }
}