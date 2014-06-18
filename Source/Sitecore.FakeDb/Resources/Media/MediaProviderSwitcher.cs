namespace Sitecore.FakeDb.Resources.Media
{
  using Sitecore.Resources.Media;

  public class MediaProviderSwitcher : ProviderBehaviorSwitcher<MediaProvider>
  {
    public MediaProviderSwitcher(MediaProvider behavior)
      : base((IBehavioral<MediaProvider>)MediaManager.Provider, behavior)
    {
    }
  }
}