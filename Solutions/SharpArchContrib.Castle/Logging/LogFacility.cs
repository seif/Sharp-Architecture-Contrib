namespace SharpArchContrib.Castle.Logging
{
    using global::Castle.Core;
    using global::Castle.MicroKernel.ModelBuilder;

    using SharpArchContrib.Core.Logging;

    public class LogFacility : AttributeControlledFacilityBase<LogAttributeSettings, LogInterceptor>
    {
        public LogFacility()
            : base(LifestyleType.Singleton)
        {
        }

        protected override void AddContributor()
        {
            this.Kernel.ComponentModelBuilder.AddContributor(new LogComponentInspector());
        }
    }
}