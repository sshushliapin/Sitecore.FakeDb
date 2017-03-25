namespace Sitecore.FakeDb.Resources.Media
{
  using System;
  using Sitecore.Resources.Media;

  public class MediaProviderSwitcher : ThreadLocalProviderSwitcher<MediaProvider>
  {
    [Obsolete("Starting from Sitecore 8.2, the " +
          "Sitecore.Resources.Media.MediaProvider " +
          "class is marked as obsolete and will be removed " +
          "in the next major release. Please use new abstract " +
          "type Sitecore.Abstractions.BaseMediaManager " +
          "from the Sitecore.Kernel assembly.")]
    public MediaProviderSwitcher(MediaProvider innerProvider)
      : base((IThreadLocalProvider<MediaProvider>)MediaManager.Provider, innerProvider)
    {
    }
  }
}