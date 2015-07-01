namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using Sitecore.Diagnostics;

  public class DbItemChildCollection : ICollection<DbItem>
  {
    private readonly DbItem parent;

    private readonly ICollection<DbItem> innerCollection;

    public DbItemChildCollection(DbItem parent)
      : this(parent, new Collection<DbItem>())
    {
    }

    public DbItemChildCollection(DbItem parent, ICollection<DbItem> innerCollection)
    {
      this.parent = parent;
      this.innerCollection = innerCollection;
    }

    public IEnumerator<DbItem> GetEnumerator()
    {
      return this.innerCollection.ToList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    public void Add(DbItem item)
    {
      Assert.ArgumentNotNull(item, "item");

      item.ParentID = this.parent.ID;
      item.FullPath = this.parent.FullPath + "/" + item.Name;

      this.innerCollection.Add(item);
    }

    public void Clear()
    {
      this.innerCollection.Clear();
    }

    public bool Contains(DbItem item)
    {
      return this.innerCollection.Contains(item);
    }

    public void CopyTo(DbItem[] array, int arrayIndex)
    {
      this.innerCollection.CopyTo(array, arrayIndex);
    }

    public bool Remove(DbItem item)
    {
      return this.innerCollection.Remove(item);
    }

    public int Count
    {
      get { return this.innerCollection.Count; }
    }

    public bool IsReadOnly
    {
      get { return this.innerCollection.IsReadOnly; }
    }
  }
}