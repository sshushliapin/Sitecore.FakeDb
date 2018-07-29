namespace Sitecore.FakeDb.AutoFixture
{
    using System;
    using global::AutoFixture.Kernel;
    using Sitecore.Data.Items;

    public class ItemSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;
            if (type == null)
            {
                return false;
            }

            return typeof(Item) == type;
        }
    }
}