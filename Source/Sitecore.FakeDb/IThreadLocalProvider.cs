namespace Sitecore.FakeDb
{
  using System.Threading;

  public interface IThreadLocalProvider<TProvider>
  {
    ThreadLocal<TProvider> LocalProvider { get; }

    bool IsLocalProviderSet();
  }
}