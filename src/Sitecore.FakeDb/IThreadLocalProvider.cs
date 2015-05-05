namespace Sitecore.FakeDb
{
  using System;
  using System.Threading;

  public interface IThreadLocalProvider<TProvider> : IDisposable
  {
    ThreadLocal<TProvider> LocalProvider { get; }

    bool IsLocalProviderSet();
  }
}