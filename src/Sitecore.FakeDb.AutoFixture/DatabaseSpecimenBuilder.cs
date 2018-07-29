namespace Sitecore.FakeDb.AutoFixture
{
    using global::AutoFixture.Kernel;
    using Sitecore.Data;
    using Sitecore.Diagnostics;

    public class DatabaseSpecimenBuilder : ISpecimenBuilder
    {
        private readonly string database;

        public DatabaseSpecimenBuilder(string database)
        {
            Assert.ArgumentNotNull(database, "database");

            this.database = database;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(Database).Equals(request))
            {
                return new NoSpecimen();
            }

            return Database.GetDatabase(this.database);
        }
    }
}