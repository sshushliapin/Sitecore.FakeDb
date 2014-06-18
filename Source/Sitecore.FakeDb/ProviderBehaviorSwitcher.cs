namespace Sitecore.FakeDb
{
  using System;
  using Sitecore.Diagnostics;

  public abstract class ProviderBehaviorSwitcher<TProvider> : IDisposable
  {
    private readonly IBehavioral<TProvider> provider;

    protected ProviderBehaviorSwitcher(IBehavioral<TProvider> provider, TProvider behavior)
    {
      Assert.ArgumentNotNull(behavior, "behavior");

      this.provider = provider;
      this.provider.Behavior = behavior;
    }

    public void Dispose()
    {
      this.provider.Behavior = default(TProvider);
    }
  }
}