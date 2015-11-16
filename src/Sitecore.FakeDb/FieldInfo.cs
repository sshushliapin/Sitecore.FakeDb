namespace Sitecore.FakeDb
{
  using System;
  using Sitecore.Data;

  public struct FieldInfo : IEquatable<FieldInfo>
  {
    public static readonly FieldInfo Empty = new FieldInfo();

    private readonly string name;

    private readonly ID id;

    private readonly bool shared;

    private readonly string type;

    public string Name
    {
      get { return this.name; }
    }

    public ID Id
    {
      get { return this.id ?? ID.Null; }
    }

    public bool Shared
    {
      get { return this.shared; }
    }

    public string Type
    {
      get { return this.type; }
    }

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

      return obj is FieldInfo && this.Equals((FieldInfo)obj);
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