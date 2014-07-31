namespace Sitecore.FakeDb
{
  using System;
  using Sitecore.Diagnostics;

  public abstract class ProviderBehaviorSwitcher<TProvider> : IDisposable
  {
    private readonly IThreadLocalProvider<TProvider> provider;

    protected ProviderBehaviorSwitcher(IThreadLocalProvider<TProvider> provider, TProvider behavior)
    {
      Assert.ArgumentNotNull(provider, "provider");
      Assert.ArgumentNotNull(behavior, "behavior");

      this.provider = provider;
      this.provider.LocalProvider.Value = behavior;
    }

    public IThreadLocalProvider<TProvider> Provider
    {
      get { return this.provider; }
    }

    public void Dispose()
    {
      this.provider.LocalProvider.Value = default(TProvider);
    }
  }
}