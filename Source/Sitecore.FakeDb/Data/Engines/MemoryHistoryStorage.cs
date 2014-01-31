namespace Sitecore.FakeDb.Data.Engines
{
  using System;
  using Sitecore.Collections;
  using Sitecore.Data.Engines;

  public class MemoryHistoryStorage : HistoryStorage
  {
    public MemoryHistoryStorage(string connectionStringName)
    {
    }

    public override void AddEntry(HistoryEntry entry)
    {

    }

    public override HistoryEntryCollection GetHistory(DateTime from, DateTime to)
    {
      return new HistoryEntryCollection();
    }
  }
}