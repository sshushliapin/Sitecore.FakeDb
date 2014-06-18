namespace Sitecore.FakeDb
{
  public interface IBehavioral<TProvider>
  {
    TProvider Behavior { get; set; }
  }
}