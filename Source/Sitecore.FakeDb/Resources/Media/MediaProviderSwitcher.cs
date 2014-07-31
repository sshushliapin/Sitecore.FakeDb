namespace Sitecore.FakeDb.Resources.Media
{
  using Sitecore.Resources.Media;

  public class MediaProviderSwitcher : ProviderBehaviorSwitcher<MediaProvider>
  {
    public MediaProviderSwitcher(MediaProvider innerProvider)
      : base((IThreadLocalProvider<MediaProvider>)MediaManager.Provider, innerProvider)
    {
    }
  }
}