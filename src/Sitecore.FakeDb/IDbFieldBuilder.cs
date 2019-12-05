namespace Sitecore.FakeDb
{
    using Sitecore.Data;

    /// <summary>
    /// Builds a <see cref="FieldInfo"/> to be used in the <see cref="DbField"/> creation.
    /// </summary>
    public interface IDbFieldBuilder
    {
        /// <summary>
        /// Builds a <see cref="FieldInfo"/> using given request. The request might be an <see cref="ID"/>,
        /// name or combination of both.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The field info.</returns>
        FieldInfo Build(object request);
    }
}