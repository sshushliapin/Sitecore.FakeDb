namespace Sitecore.FakeDb.Resources.Media
{
  using System;
  using System.Web;
  using Sitecore.Data.Items;
  using Sitecore.Resources.Media;

  public class StubMediaProvider : MediaProvider
  {
    private readonly MediaCache mediaCache = new MediaCache();

    private readonly MediaConfig mediaConfig = new MediaConfig();

    private readonly MediaCreator mediaCreator = new MediaCreator();

    private readonly ImageEffects imageEffects = new ImageEffects();

    private readonly MimeResolver mimeResolver = new MimeResolver();

    private readonly Media getMedia = new Media();

    private readonly MediaRequest parseMediaRequest = new MediaRequest();

    public override MediaCache Cache
    {
      get { return this.mediaCache; }
      set { throw new NotSupportedException(); }
    }

    public override MediaConfig Config
    {
      get { return this.mediaConfig; }
      set { throw new NotSupportedException(); }
    }

    public override MediaCreator Creator
    {
      get { return this.mediaCreator; }
      set { throw new NotSupportedException(); }
    }

    public override ImageEffects Effects
    {
      get { return this.imageEffects; }
      set { throw new NotSupportedException(); }
    }

    public override string MediaLinkPrefix
    {
      get { return string.Empty; }
    }

    public override MimeResolver MimeResolver
    {
      get { return this.mimeResolver; }
      set { throw new NotSupportedException(); }
    }

    public override Media GetMedia(MediaItem item)
    {
      return this.getMedia;
    }

    public override Media GetMedia(MediaUri mediaUri)
    {
      return this.getMedia;
    }

    public override string GetMediaUrl(MediaItem item)
    {
      return string.Empty;
    }

    public override string GetMediaUrl(MediaItem item, MediaUrlOptions options)
    {
      return string.Empty;
    }

    public override string GetThumbnailUrl(MediaItem item)
    {
      return string.Empty;
    }

    public override bool HasMediaContent(Item item)
    {
      return false;
    }

    public override bool IsMediaRequest(HttpRequest httpRequest)
    {
      return false;
    }

    public override bool IsMediaUrl(string url)
    {
      return false;
    }

    public override MediaRequest ParseMediaRequest(HttpRequest request)
    {
      return this.parseMediaRequest;
    }
  }
}