namespace Sitecore.FakeDb.Resources.Media
{
  using System;
  using System.Threading;
  using System.Web;
  using Sitecore.Data.Items;
  using Sitecore.Resources.Media;

  public class FakeMediaProvider : MediaProvider, IThreadLocalProvider<MediaProvider>
  {
    private readonly ImageEffects imageEffects = new ImageEffects();

    private readonly ThreadLocal<MediaProvider> localProvider = new ThreadLocal<MediaProvider>();

    private readonly MediaCache mediaCache = new MediaCache();

    private readonly MediaConfig mediaConfig = new MediaConfig();

    private readonly MediaCreator mediaCreator = new MediaCreator();

    private readonly MimeResolver mimeResolver = new MimeResolver();

    private bool disposed;

    public override MediaCache Cache
    {
      get
      {
        return this.IsLocalProviderSet() ? this.localProvider.Value.Cache : this.mediaCache;
      }

      set
      {
        if (this.IsLocalProviderSet())
        {
          this.localProvider.Value.Cache = value;
        }
      }
    }

    public override MediaConfig Config
    {
      get
      {
        return this.IsLocalProviderSet() ? this.localProvider.Value.Config : this.mediaConfig;
      }

      set
      {
        if (this.IsLocalProviderSet())
        {
          this.localProvider.Value.Config = value;
        }
      }
    }

    public override MediaCreator Creator
    {
      get
      {
        return this.IsLocalProviderSet() ? this.localProvider.Value.Creator : this.mediaCreator;
      }

      set
      {
        if (this.IsLocalProviderSet())
        {
          this.localProvider.Value.Creator = value;
        }
      }
    }

    public override ImageEffects Effects
    {
      get
      {
        return this.IsLocalProviderSet() ? this.localProvider.Value.Effects : this.imageEffects;
      }

      set
      {
        if (this.IsLocalProviderSet())
        {
          this.localProvider.Value.Effects = value;
        }
      }
    }

    public virtual ThreadLocal<MediaProvider> LocalProvider
    {
      get { return this.localProvider; }
    }

    public override string MediaLinkPrefix
    {
      get { return this.IsLocalProviderSet() ? this.localProvider.Value.MediaLinkPrefix : string.Empty; }
    }

    public override MimeResolver MimeResolver
    {
      get
      {
        return this.IsLocalProviderSet() ? this.localProvider.Value.MimeResolver : this.mimeResolver;
      }

      set
      {
        if (this.IsLocalProviderSet())
        {
          this.localProvider.Value.MimeResolver = value;
        }
      }
    }

    public override Media GetMedia(MediaItem item)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetMedia(item) : null;
    }

    public override Media GetMedia(MediaUri mediaUri)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetMedia(mediaUri) : null;
    }

    public override string GetMediaUrl(MediaItem item)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetMediaUrl(item) : null;
    }

    public override string GetMediaUrl(MediaItem item, MediaUrlOptions options)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetMediaUrl(item, options) : null;
    }

    public override string GetThumbnailUrl(MediaItem item)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetThumbnailUrl(item) : null;
    }

    public override bool HasMediaContent(Item item)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.HasMediaContent(item);
    }

    public virtual bool IsLocalProviderSet()
    {
      return this.localProvider.Value != null;
    }

    public override bool IsMediaRequest(HttpRequest httpRequest)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.IsMediaRequest(httpRequest);
    }

    public override bool IsMediaUrl(string url)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.IsMediaUrl(url);
    }

    public override MediaRequest ParseMediaRequest(HttpRequest request)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.ParseMediaRequest(request) : null;
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

      this.localProvider.Dispose();

      this.disposed = true;
    }
  }
}