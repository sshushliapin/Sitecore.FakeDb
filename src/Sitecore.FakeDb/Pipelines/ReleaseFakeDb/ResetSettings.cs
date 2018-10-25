namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
    using Sitecore.Configuration;
    using Sitecore.Pipelines;

    public class ResetSettings
    {
        public void Process(PipelineArgs args)
        {
#pragma warning disable 618
            Settings.Reset();
#pragma warning restore 618
        }
    }
}