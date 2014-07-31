namespace Sitecore.FakeDb.Tests
{
  using System.Configuration.Provider;
  using System.Threading;
  using FluentAssertions;
  using NSubstitute;
  using Xunit;

  public class ProviderBehaviorSwithcerTest
  {
    [Fact]
    public void ShouldSetProviderBehaviour()
    {
      // arrange
      var provider = Substitute.For<IThreadLocalProvider<ProviderBase>, ProviderBase>();
      provider.LocalProvider.Returns(Substitute.For<ThreadLocal<ProviderBase>>());
      var befaviour = Substitute.For<ProviderBase>();

      // act
      var swithcer = new TestProviderBehaviorSwithcer<ProviderBase>(provider, befaviour);

      // assert
      swithcer.Provider.Should().Be(provider);
      swithcer.Provider.LocalProvider.Value.Should().Be(befaviour);
    }

    [Fact]
    public void ShouldResetProviderBehaviourOnDispose()
    {
      // arrange
      var provider = Substitute.For<IThreadLocalProvider<ProviderBase>, ProviderBase>();
      provider.LocalProvider.Returns(Substitute.For<ThreadLocal<ProviderBase>>());
      var befaviour = Substitute.For<ProviderBase>();

      var swithcer = new TestProviderBehaviorSwithcer<ProviderBase>(provider, befaviour);

      // act
      swithcer.Dispose();

      // assert
      swithcer.Provider.LocalProvider.Value.Should().BeNull();
    }

    private class TestProviderBehaviorSwithcer<TProvider> : ProviderBehaviorSwitcher<TProvider>
    {
      public TestProviderBehaviorSwithcer(IThreadLocalProvider<TProvider> provider, TProvider behavior)
        : base(provider, behavior)
      {
      }
    }
  }
}