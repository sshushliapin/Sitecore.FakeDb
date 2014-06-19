namespace Sitecore.FakeDb
{
  using System;
  using Sitecore.Diagnostics;

  public abstract class ProviderBehaviorSwitcher<TProvider> : IDisposable
  {
    private readonly IBehavioral<TProvider> provider;

    protected ProviderBehaviorSwitcher(IBehavioral<TProvider> provider, TProvider behavior)
    {
      Assert.ArgumentNotNull(provider, "provider");
      Assert.ArgumentNotNull(behavior, "behavior");

      this.provider = provider;
      this.provider.Behavior = behavior;
    }

    public IBehavioral<TProvider> Provider
    {
      get { return this.provider; }
    }

    public void Dispose()
    {
      this.provider.Behavior = default(TProvider);
    }
  }
}