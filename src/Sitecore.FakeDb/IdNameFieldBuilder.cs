namespace Sitecore.FakeDb
{
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Data;
    using Sitecore.Diagnostics;

    /// <summary>
    /// Builds a <see cref="FieldInfo"/> to be used in the <see cref="DbField"/> creation based on the
    /// predefined name and auto-generated <see cref="ID"/>.
    /// </summary>
    public class IdNameFieldBuilder : IDbFieldBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdBasedStandardFieldResolver"/> class.
        /// </summary>
        /// <param name="nameBuilder">The name field info builder.</param>
        /// <param name="idBuilder">The id field info builder.</param>
        public IdNameFieldBuilder(IDbFieldBuilder nameBuilder, IDbFieldBuilder idBuilder)
        {
            Assert.ArgumentNotNull(nameBuilder, "nameBuilder");
            Assert.ArgumentNotNull(idBuilder, "idBuilder");

            this.NameBuilder = nameBuilder;
            this.IdBuilder = idBuilder;
        }

        /// <summary>
        /// Gets the name field info builder.
        /// </summary>
        public IDbFieldBuilder NameBuilder { get; private set; }

        /// <summary>
        /// Gets the id field info builder.
        /// </summary>
        public IDbFieldBuilder IdBuilder { get; private set; }

        public FieldInfo Build(object request)
        {
            var mixedRequest = request as IEnumerable<object>;
            if (mixedRequest == null)
            {
                return FieldInfo.Empty;
            }

            var list = mixedRequest.ToList();
            var nameInfo = this.NameBuilder.Build(list.FirstOrDefault(x => x is string));
            var idInfo = this.IdBuilder.Build(list.FirstOrDefault(x => x is ID));

            if (nameInfo == FieldInfo.Empty && idInfo == FieldInfo.Empty)
            {
                return FieldInfo.Empty;
            }

            return new FieldInfo(nameInfo.Name, idInfo.Id, nameInfo.Shared, nameInfo.Type);
        }
    }
}