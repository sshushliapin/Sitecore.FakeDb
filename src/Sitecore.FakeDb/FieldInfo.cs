namespace Sitecore.FakeDb
{
  using System;

  internal struct FieldInfo : IEquatable<FieldInfo>
  {
    public static readonly FieldInfo Empty = new FieldInfo();

    public readonly string Name;

    public readonly Guid Id;

    public readonly bool Shared;

    public readonly string Type;

    public FieldInfo(string name, Guid id, bool shared, string type)
    {
      this.Name = name;
      this.Id = id;
      this.Shared = shared;
      this.Type = type;
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

      return obj is FieldInfo && this.Equals((FieldInfo)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ this.Id.GetHashCode();
        hashCode = (hashCode * 397) ^ this.Shared.GetHashCode();
        hashCode = (hashCode * 397) ^ (this.Type != null ? this.Type.GetHashCode() : 0);
        return hashCode;
      }
    }

    public static bool operator ==(FieldInfo a, FieldInfo b)
    {
      return a.Name == b.Name && a.Id == b.Id && a.Shared == b.Shared && a.Type == b.Type;
    }

    public static bool operator !=(FieldInfo a, FieldInfo b)
    {
      return !(a == b);
    }
  }
}