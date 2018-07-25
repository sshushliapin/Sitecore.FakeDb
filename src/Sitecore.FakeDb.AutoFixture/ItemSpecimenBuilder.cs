namespace Sitecore.FakeDb.AutoFixture
{
  using global::AutoFixture.Kernel;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;

  /// <summary>
  /// Creates new <see cref="Item"/> instances.
  /// </summary>
  public class ItemSpecimenBuilder : ISpecimenBuilder
  {
    /// <summary>
    /// Creates a new <see cref="Item"/> instance.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">The specimen context. Used to resolve the <see cref="Item"/> dependencies.</param>
    /// <returns>
    /// A new <see cref="Item"/> instance, if <paramref name="request"/> is a request for a
    /// <see cref="Item"/>; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
      if (!typeof(Item).Equals(request))
      {
        return new NoSpecimen();
      }

      var database = (Database)context.Resolve(typeof(Database));
      var name = (string)context.Resolve(typeof(string));
      var itemId = (ID)context.Resolve(typeof(ID));
      var templateId = (ID)context.Resolve(typeof(ID));
      var branchId = (ID)context.Resolve(typeof(ID));
      var fields = (FieldList)context.Resolve(typeof(FieldList));
      var language = (Language)context.Resolve(typeof(Language));
      var version = (Version)context.Resolve(typeof(Version));

      return ItemHelper.CreateInstance(database, name, itemId, templateId, branchId, fields, language, version);
    }
  }
}