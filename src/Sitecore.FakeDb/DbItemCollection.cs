namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;

  public class DbItemCollection : ICollection<DbItem>
  {
    private readonly ICollection<DbItem> innerCollection;

    public DbItemCollection()
      : this(new Collection<DbItem>())
    {
    }

    public DbItemCollection(ICollection<DbItem> innerCollection)
    {
      this.innerCollection = innerCollection;
    }

    public IEnumerator<DbItem> GetEnumerator()
    {
      return this.innerCollection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    public void Add(DbItem item)
    {
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