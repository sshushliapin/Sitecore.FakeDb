namespace Examples
{
  using global::NSubstitute;
  using Xunit;

  public class GettingStarted
  {
    [Fact]
    public void HowDoIMockBucketProvider()
    {
      // act
      Sitecore.Buckets.Managers.BucketManager.AddSearchTabToItem(null);

      // assert
      Sitecore.Buckets.Managers.BucketManager.Provider.Received().AddSearchTabToItem(null);
    }
  }
}