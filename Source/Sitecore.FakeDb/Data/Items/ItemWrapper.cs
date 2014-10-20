namespace Sitecore.FakeDb.Data.Items
{
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// A Sitecore Item that overrides Equals and HashCode to go by the item's ID.
  /// 
  /// FakeDb creates item instances every time a code asks for one and there are cases when not having
  /// two otherwise identical items be "equal" hurts. 
  /// </summary>
  public class ItemWrapper : Item
  {
    public ItemWrapper(ID itemID, ItemData data, Database database)
      : base(itemID, data, database)
    {
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        return false;
      }

      var other = obj as ItemWrapper;
      if (other == null)
      {
        return false;
      }

      return ID.Equals(other.ID);
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }
  }
}