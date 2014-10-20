namespace Sitecore.FakeDb.Tests
{
  using System.Configuration.Provider;
  using System.Threading;
  using FluentAssertions;
  using NSubstitute;
  using Xunit;

  public class ThreadLocalProviderSwitcherTest
  {
    [Fact]
    public void ShouldSetLocalProvider()
    {
      // arrange
      var provider = Substitute.For<IThreadLocalProvider<ProviderBase>, ProviderBase>();
      provider.LocalProvider.Returns(Substitute.For<ThreadLocal<ProviderBase>>());
      var befaviour = Substitute.For<ProviderBase>();

      // act
      var swithcer = new SampleThreadLocalProviderSwithcer<ProviderBase>(provider, befaviour);

      // assert
      swithcer.Provider.Should().Be(provider);
      swithcer.Provider.LocalProvider.Value.Should().Be(befaviour);
    }

    [Fact]
    public void ShouldResetLocalProviderOnDispose()
    {
      // arrange
      var provider = Substitute.For<IThreadLocalProvider<ProviderBase>, ProviderBase>();
      provider.LocalProvider.Returns(Substitute.For<ThreadLocal<ProviderBase>>());
      var befaviour = Substitute.For<ProviderBase>();

      var swithcer = new SampleThreadLocalProviderSwithcer<ProviderBase>(provider, befaviour);

      // act
      swithcer.Dispose();

      // assert
      swithcer.Provider.LocalProvider.Value.Should().BeNull();
    }

    private class SampleThreadLocalProviderSwithcer<TProvider> : ThreadLocalProviderSwitcher<TProvider>
    {
      public SampleThreadLocalProviderSwithcer(IThreadLocalProvider<TProvider> rootProvider, TProvider localProvider)
        : base(rootProvider, localProvider)
      {
      }
    }
  }
}