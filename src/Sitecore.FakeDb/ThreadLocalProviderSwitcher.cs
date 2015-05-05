namespace Sitecore.FakeDb
{
  using System;
  using Sitecore.Diagnostics;

  public abstract class ThreadLocalProviderSwitcher<TProvider> : IDisposable
  {
    private readonly IThreadLocalProvider<TProvider> provider;

    private bool disposed;

    protected ThreadLocalProviderSwitcher(IThreadLocalProvider<TProvider> provider, TProvider localProvider)
    {
      Assert.ArgumentNotNull(provider, "provider");
      Assert.ArgumentNotNull(localProvider, "localProvider");

      this.provider = provider;

      Assert.IsNotNull(this.provider.LocalProvider, "provider.LocalProvider is not set.");
      this.provider.LocalProvider.Value = localProvider;
    }

    public IThreadLocalProvider<TProvider> Provider
    {
      get { return this.provider; }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (!disposing)
      {
        return;
      }

      this.provider.LocalProvider.Value = default(TProvider);

      this.disposed = true;
    }
  }
}