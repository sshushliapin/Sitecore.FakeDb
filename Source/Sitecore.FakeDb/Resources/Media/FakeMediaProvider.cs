namespace Sitecore.FakeDb.Resources.Media
{
  using System.Threading;
  using System.Web;
  using Sitecore.Data.Items;
  using Sitecore.Resources.Media;

  public class FakeMediaProvider : MediaProvider
  {
    private static readonly MediaProvider Stub = new StubMediaProvider();

    private readonly ThreadLocal<MediaProvider> behavior = new ThreadLocal<MediaProvider>();

    public MediaProvider Behavior
    {
      get { return this.behavior.Value ?? Stub; }
      set { this.behavior.Value = value; }
    }

    public override MediaCache Cache
    {
      get { return this.Behavior.Cache; }
      set { this.Behavior.Cache = value; }
    }

    public override MediaConfig Config
    {
      get { return this.Behavior.Config; }
      set { this.Behavior.Config = value; }
    }

    public override MediaCreator Creator
    {
      get { return this.Behavior.Creator; }
      set { this.Behavior.Creator = value; }
    }

    public override ImageEffects Effects
    {
      get { return this.Behavior.Effects; }
      set { this.Behavior.Effects = value; }
    }

    public override string MediaLinkPrefix
    {
      get { return this.Behavior.MediaLinkPrefix; }
    }

    public override MimeResolver MimeResolver
    {
      get { return this.Behavior.MimeResolver; }
      set { this.Behavior.MimeResolver = value; }
    }

    public override Media GetMedia(MediaItem item)
    {
      return this.Behavior.GetMedia(item);
    }

    public override Media GetMedia(MediaUri mediaUri)
    {
      return this.Behavior.GetMedia(mediaUri);
    }

    public override string GetMediaUrl(MediaItem item)
    {
      return this.Behavior.GetMediaUrl(item);
    }

    public override string GetMediaUrl(MediaItem item, MediaUrlOptions options)
    {
      return this.Behavior.GetMediaUrl(item, options);
    }

    public override string GetThumbnailUrl(MediaItem item)
    {
      return this.Behavior.GetThumbnailUrl(item);
    }

    public override bool HasMediaContent(Item item)
    {
      return this.Behavior.HasMediaContent(item);
    }

    public override bool IsMediaRequest(HttpRequest httpRequest)
    {
      return this.Behavior.IsMediaRequest(httpRequest);
    }

    public override bool IsMediaUrl(string url)
    {
      return this.Behavior.IsMediaUrl(url);
    }

    public override MediaRequest ParseMediaRequest(HttpRequest request)
    {
      return this.Behavior.ParseMediaRequest(request);
    }
  }
}