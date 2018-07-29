namespace Sitecore.FakeDb
{
    using System;
    using Sitecore.Data;

    /// <summary>
    /// Contains basic field info.
    /// </summary>
    public struct FieldInfo : IEquatable<FieldInfo>
    {
        /// <summary>
        /// The empty field info.
        /// </summary>
        public static readonly FieldInfo Empty = new FieldInfo();

        private readonly string name;

        private readonly ID id;

        private readonly bool shared;

        private readonly string type;

        /// <summary>
        /// Gets the field name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the field id.
        /// </summary>
        public ID Id
        {
            get { return this.id ?? ID.Null; }
        }

        /// <summary>
        /// Gets the field shared info.
        /// </summary>
        public bool Shared
        {
            get { return this.shared; }
        }

        /// <summary>
        /// Gets the field type.
        /// </summary>
        public string Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeFieldBuilder"/> struct.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="id">The field id.</param>
        /// <param name="shared">The field shared.</param>
        /// <param name="type">The field type.</param>
        public FieldInfo(string name, ID id, bool shared, string type)
        {
            this.name = name;
            this.id = id;
            this.shared = shared;
            this.type = type;
        }

        public bool Equals(FieldInfo other)
        {
            return string.Equals(this.Name, other.Name) &&
                   this.Id.Equals(other.Id) &&
                   this.Shared == other.Shared &&
                   string.Equals(this.Type, other.Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is FieldInfo && this.Equals((FieldInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Name != null ? this.Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Shared.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Type != null ? this.Type.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        /// Determines if the two fields are equals.
        /// </summary>
        /// <param name="a">The field 'a'.</param>
        /// <param name="b">The field 'b'.</param>
        /// <returns>True if equals, otherwise false.</returns>
        public static bool operator ==(FieldInfo a, FieldInfo b)
        {
            return a.Name == b.Name && a.Id == b.Id && a.Shared == b.Shared && a.Type == b.Type;
        }

        /// <summary>
        /// Determines if the two fields are not equal.
        /// </summary>
        /// <param name="a">The field 'a'.</param>
        /// <param name="b">The field 'b'.</param>
        /// <returns>True if not equal, otherwise false.</returns>
        public static bool operator !=(FieldInfo a, FieldInfo b)
        {
            return !(a == b);
        }
    }
}